import { AuthContextProps, AuthProvider, useAuth, User, UserManager } from 'oidc-react';
import React, { useCallback, useContext, useEffect, useState } from 'react';
import { useQuery } from 'react-query';
import { useHistory, useLocation } from 'react-router-dom';
import Profile from '../../Common/Interfaces/Profile';
import { HTTPStatusCodes, useApiCustomAuth, HTTPError } from '../../Hooks/useApi';
import { useAppContext } from '../AppContext/AppContextProvider';

interface IAuthWrapperContext {
    loading: boolean;
    isError: boolean;
    profileExists: boolean;
}
export const AuthWrapperContext = React.createContext<IAuthWrapperContext | null>(null);

export const AuthWrapper: React.FC = ({ children }) => {
    const {
        authority: { host, scope, clientId, extraParams },
        host: { client: clientHost },
    } = useAppContext();
    const { replace } = useHistory();
    const { search, pathname } = useLocation();
    const [initializing, setInitializing] = useState(true);
    const [user, setUser] = useState<User | null>(null);
    const [silentRenewFailed, setSilentRenewFailed] = useState<boolean>(false);

    const [userManager] = useState<UserManager>(
        new UserManager({
            scope,
            response_type: 'code',
            redirect_uri: `${clientHost}/oidc-signin`,
            automaticSilentRenew: true,
            silent_redirect_uri: `${clientHost}/oidc-silent-renew`,
            post_logout_redirect_uri: clientHost,
            filterProtocolClaims: true,
            loadUserInfo: true,
            authority: host,
            client_id: clientId,
            extraQueryParams: extraParams ?? undefined,
        }),
    );

    useEffect(() => {
        userManager.clearStaleState();
    }, [userManager]);

    const onSessionEnd = useCallback(() => {
        setUser(null);
        userManager.signoutRedirect();
    }, [userManager]);

    const onSilentRenewFailed = useCallback(() => setSilentRenewFailed(true), [setSilentRenewFailed]);

    useEffect(() => {
        userManager.events.addSilentRenewError(onSilentRenewFailed);
        userManager.events.addAccessTokenExpired(onSessionEnd);

        return () => {
            userManager.events.removeSilentRenewError(onSilentRenewFailed);
            userManager.events.removeAccessTokenExpired(onSessionEnd);
        };
    }, [userManager, onSessionEnd, onSilentRenewFailed]);

    const api = useApiCustomAuth(user?.access_token);
    const {
        refetch: apiLogin,
        isLoading: apiLoginLoading,
        data: apiLoginData,
        error: apiLoginError,
    } = useQuery<Profile, HTTPError>(
        'fetchCurrentUser',
        async () => {
            const res = await api.get(`auth/user`);
            return await res.json();
        },
        {
            enabled: false,
            onError: (error) => {
                if (error.response.status === HTTPStatusCodes.Status401) {
                    userManager.signoutRedirect();
                }
            },
        },
    );

    useEffect(() => {
        if (user && initializing) apiLogin();
    }, [user]);

    const profileDoesNotExist =
        initializing || apiLoginLoading
            ? undefined
            : apiLoginError?.response?.status === HTTPStatusCodes.Status404 ?? undefined;

    useEffect(() => {
        if (user && initializing) {
            setInitializing(false);
            if (apiLoginData) {
                if (user?.state?.targetUrl !== undefined && pathname !== user?.state?.targetUrl) {
                    replace(user?.state?.targetUrl);
                } else {
                    replace(window.location.pathname); // remove code query param
                }
            }
        } else if (profileDoesNotExist) {
            replace('/profile/create');
        }

        if (apiLoginError) {
            setInitializing(false);
        }
    }, [user, apiLoginData, apiLoginError, profileDoesNotExist, replace, pathname]);

    const onSignIn = useCallback(
        (user: User | null) => {
            setUser(user);
            if (!user) {
                setInitializing(false);
            }
        },
        [apiLogin],
    );

    const urlParams = new URLSearchParams(search);
    const isCodeCallback = urlParams.has('code');

    useEffect(() => {
        const signInSilent = async () => {
            try {
                if (!silentRenewFailed) {
                    const user = await userManager.signinSilent();
                    if (user === null) onSilentRenewFailed();

                    onSignIn(user);
                }
            } catch (error) {
                onSilentRenewFailed();
            }
        };

        const getSession = async () => {
            const user = await userManager.getUser();
            onSignIn(user);

            if (user === null && !isCodeCallback) await signInSilent();
        };

        if (!isCodeCallback) getSession();
        //eslint-disable-next-line
    }, []);

    const loading = initializing || apiLoginLoading || (!user && isCodeCallback);
    const isError = apiLoginError && !profileDoesNotExist;

    return (
        <AuthProvider userManager={userManager} autoSignIn={false} onSignIn={onSignIn}>
            <AuthWrapperContext.Provider
                value={{
                    loading,
                    isError: isError ?? false,
                    profileExists: !profileDoesNotExist,
                }}
            >
                {children}
            </AuthWrapperContext.Provider>
        </AuthProvider>
    );
};

interface BaseState {
    isLoading: boolean;
    isLoggedIn: undefined;
    isError: boolean;
}

interface UnauthenticatedState extends Omit<BaseState, 'isLoggedIn'> {
    isLoggedIn: false;
    signIn: AuthContextProps['signIn'];
}

export interface AuthenticatedState extends Omit<BaseState, 'isLoggedIn'> {
    isLoggedIn: true;
    oidcProfile: User['profile'];
    bearerToken: string;
    appProfileExists: boolean;
    signOut: AuthContextProps['signOut'];
}

export const useAuthentication = (): BaseState | UnauthenticatedState | AuthenticatedState => {
    const { userData, signIn, signOutRedirect } = useAuth();
    const authWrapperContext = useContext(AuthWrapperContext);

    if (!authWrapperContext) throw new Error('AuthWrapperContext not registered');

    const signOutAndRedirect = async (args?: unknown) => {
        window.localStorage.clear();
        await signOutRedirect(args);
    };

    if (authWrapperContext.loading) {
        return { isLoading: true, isError: false, isLoggedIn: undefined };
    }

    if (authWrapperContext.isError) {
        return { isLoading: false, isError: true, isLoggedIn: undefined };
    }

    if (userData) {
        return {
            isLoading: false,
            isError: false,
            isLoggedIn: true,
            bearerToken: userData.access_token,
            appProfileExists: authWrapperContext.profileExists,
            oidcProfile: userData.profile,
            signOut: signOutAndRedirect,
        };
    }

    return {
        isLoading: false,
        isError: false,
        isLoggedIn: false,
        signIn: signIn,
    };
};

export const useAuthenticatedState = (): AuthenticatedState => {
    const authState = useAuthentication();

    if (!authState.isLoggedIn) {
        throw new Error('Expected user to be logged in');
    }

    return authState;
};

export const useUnauthenticatedState = (): UnauthenticatedState => {
    const authState = useAuthentication();

    if (authState.isLoggedIn !== false) {
        throw new Error('Expected user not to be logged in');
    }

    return authState;
};

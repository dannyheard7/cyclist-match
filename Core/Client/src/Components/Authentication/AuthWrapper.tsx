import { User, UserManager } from 'oidc-client';
import React, { useCallback, useContext, useEffect, useState } from 'react';
import { useQuery } from 'react-query';
import { useNavigate, useLocation } from 'react-router-dom';
import Profile from '../../Common/Interfaces/Profile';
import { HTTPStatusCodes, useApiCustomAuth, HTTPError } from '../../Hooks/useApi';
import { useAppContext } from '../AppContext/AppContextProvider';
interface IAuthWrapperContext {
    loading: boolean;
    isError: boolean;
    signIn: (args?: unknown) => void;
    signOut: (args?: unknown) => void;
    oidcUser: User | null;
    profile: Profile | null;
}

export const AuthWrapperContext = React.createContext<IAuthWrapperContext | null>(null);

export const AuthWrapper: React.FC<{ children?: React.ReactNode }> = ({ children }) => {
    const {
        authority: { host, scope, clientId, extraParams },
        host: { client: clientHost },
    } = useAppContext();
    const replace = useNavigate();
    const { search, pathname } = useLocation();
    const [initializing, setInitializing] = useState(true);
    const [user, setUser] = useState<User | null>(null);

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

    const onUserSignedIn = useCallback(async () => {
        const user = await userManager.getUser();
        setUser(user);
    }, [userManager]);

    const onUserSignedOut = () => setUser(null);

    const onSilentRenewFailed = useCallback(() => {
        setUser(null);
        userManager.signoutRedirect();
    }, [userManager]);

    useEffect(() => {
        userManager.events.addUserSignedIn(onUserSignedIn);
        userManager.events.addUserSignedOut(onUserSignedOut);
        userManager.events.addSilentRenewError(onSilentRenewFailed);
        userManager.events.addAccessTokenExpired(onSessionEnd);

        return () => {
            userManager.events.removeUserSignedIn(onUserSignedIn);
            userManager.events.removeUserSignedOut(onUserSignedOut);
            userManager.events.removeSilentRenewError(onSilentRenewFailed);
            userManager.events.removeAccessTokenExpired(onSessionEnd);
        };
    }, [userManager, onSessionEnd, onSilentRenewFailed, onUserSignedIn]);

    const api = useApiCustomAuth(user?.access_token);
    const {
        refetch: apiLogin,
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
            onSettled: () => {
                setInitializing(false);
            },
            onSuccess: (_) => {
                if (user?.state?.targetUrl !== undefined && pathname !== user?.state?.targetUrl) {
                    replace(user?.state?.targetUrl);
                } else if (isCodeCallback) {
                    replace('/');
                }
            },
            onError: (error) => {
                if (error.response.status === HTTPStatusCodes.Status404) {
                    replace('/profile/create');
                } else if (error.response.status === HTTPStatusCodes.Status401) {
                    userManager.signoutRedirect();
                }
            },
        },
    );

    useEffect(() => {
        if (user && initializing) {
            apiLogin();
        } else if (!user && !initializing) {
            replace('/');
        }
        //eslint-disable-next-line
    }, [user, initializing]);

    const onSignIn = useCallback((user: User | null) => {
        setUser(user);
        if (!user) {
            setInitializing(false);
        }
    }, []);

    const urlParams = new URLSearchParams(search);
    const isCodeCallback = urlParams.has('code');

    useEffect(() => {
        const signInSilent = async () => {
            try {
                const user = await userManager.signinSilent();
                onSignIn(user);
            } catch {
                setInitializing(false);
            }
        };

        const initialize = async () => {
            if (isCodeCallback) {
                const user = await userManager.signinRedirectCallback();
                onSignIn(user);
            } else {
                const user = await userManager.getUser();
                if (user === null) {
                    await signInSilent();
                } else {
                    onSignIn(user);
                }
            }
        };

        initialize();
        //eslint-disable-next-line
    }, []);

    const profileDoesNotExist = apiLoginError?.response?.status === HTTPStatusCodes.Status404;
    const isError = apiLoginError && !profileDoesNotExist;

    return (
        <AuthWrapperContext.Provider
            value={{
                loading: initializing,
                oidcUser: user,
                signIn: () => userManager.signinRedirect(),
                signOut: () => userManager.signoutRedirect(),
                isError: isError ?? false,
                profile: apiLoginData ?? null,
            }}
        >
            {children}
        </AuthWrapperContext.Provider>
    );
};

interface BaseState {
    isLoading: boolean;
    isLoggedIn: undefined;
    isError: boolean;
}

interface UnauthenticatedState extends Omit<BaseState, 'isLoggedIn'> {
    isLoggedIn: false;
    signIn: IAuthWrapperContext['signIn'];
}

export interface AuthenticatedState extends Omit<BaseState, 'isLoggedIn'> {
    isLoggedIn: true;
    oidcProfile: User['profile'];
    bearerToken: string;
    appProfileExists: boolean;
    signOut: IAuthWrapperContext['signOut'];
}

export const useAuthentication = (): BaseState | UnauthenticatedState | AuthenticatedState => {
    const authWrapperContext = useContext(AuthWrapperContext);
    if (!authWrapperContext) throw new Error('AuthWrapperContext not registered');
    const { signOut, oidcUser: userData, signIn } = authWrapperContext;

    const signOutAndRedirect = async (args?: unknown) => {
        window.localStorage.clear();
        await signOut(args);
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
            appProfileExists: authWrapperContext.profile != null,
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

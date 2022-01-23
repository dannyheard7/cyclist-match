import ky from 'ky';
import { AuthProvider, AuthProviderProps, useAuth, User, UserManager } from 'oidc-react';
import React, { useCallback, useEffect, useState } from 'react';
import { useMutation } from 'react-query';
import { useHistory, useLocation } from 'react-router-dom';
import Profile from '../../Common/Interfaces/Profile';
import { useApiCustomAuth } from '../../Hooks/useApi';
import { useAppContext } from '../AppContext/AppContextProvider';
import ErrorMessage from '../ErrorMessage/ErrorMessage';
import Loading from '../Loading/Loading';

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
        mutate: apiLogin,
        isLoading: apiLoginLoading,
        data: apiLoginData,
        error: apiLoginError,
    } = useMutation<Profile, ky.HTTPError>(async () => {
        const res = await api.post(`auth/login`);
        return await res.json();
    });

    const profileDoesNotExist = apiLoginError?.response?.status === 404;
    useEffect(() => {
        if (user) {
            setInitializing(false);
            if (apiLoginData) {
                if (user?.state?.targetUrl !== undefined && pathname !== user?.state?.targetUrl) {
                    replace(user?.state?.targetUrl);
                } else {
                    replace(window.location.pathname); // remove code query param
                }
            } else if (profileDoesNotExist) {
                replace('/profile/create');
            }
        }

        if (apiLoginError) {
            setInitializing(false);
        }
    }, [user, apiLoginData, apiLoginError, profileDoesNotExist, replace, pathname]);

    const onSignIn = useCallback(
        (user: User | null) => {
            setUser(user);
            if (user) {
                apiLogin();
            } else {
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

    const oidcConfig: AuthProviderProps = {
        userManager: userManager,
        autoSignIn: false,
        onSignIn,
    };

    return <AuthProvider {...oidcConfig}>{loading ? <Loading /> : isError ? <ErrorMessage /> : children}</AuthProvider>;
};

export const useAuthentication = () => {
    const { userData, signIn, signOutRedirect, userManager } = useAuth();

    const signOutAndRedirect = () => {
        window.localStorage.clear();
        signOutRedirect({ id_token_hint: userData?.id_token });
    };

    const refreshSession = () => {
        userManager.signinSilent().catch(() => signOutAndRedirect());
    };

    return {
        bearerToken: userData?.access_token,
        isLoggedIn: userData !== undefined && userData !== null,
        profile: userData?.profile,
        signInRedirect: signIn,
        refreshSession,
        signOut: signOutAndRedirect,
    };
};

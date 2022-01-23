import { UserManagerSettings } from 'oidc-client';
import { AuthProvider, AuthProviderProps, useAuth, User, UserManager } from 'oidc-react';
import React, { useCallback, useEffect, useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import { useAppContext } from '../AppContext/AppContextProvider';
import Loading from '../Loading/Loading';

interface Props {
    settings?: Partial<UserManagerSettings>;
}

export const AuthWrapper: React.FC<Props> = ({ children, settings }) => {
    const {
        authority: { host, scope, clientId, audience },
        host: { client: clientHost },
    } = useAppContext();
    const { replace } = useHistory();
    const { search, pathname } = useLocation();
    const [initializing, setInitializing] = useState(true);
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [silentRenewFailed, setSilentRenewFailed] = useState<boolean>(false);

    const urlParams = new URLSearchParams(search);
    const isCodeCallback = urlParams.has('code');

    const [userManager] = useState<UserManager>(
        new UserManager({
            scope: settings?.scope ?? scope,
            response_type: settings?.response_type ?? 'code',
            redirect_uri: settings?.redirect_uri ?? `${clientHost}/oidc-signin`,
            automaticSilentRenew: settings?.automaticSilentRenew ?? true,
            silent_redirect_uri: settings?.silent_redirect_uri ?? `${clientHost}/oidc-silent-renew`,
            post_logout_redirect_uri: settings?.post_logout_redirect_uri ?? clientHost,
            filterProtocolClaims: settings?.filterProtocolClaims ?? true,
            loadUserInfo: settings?.filterProtocolClaims ?? true,
            authority: settings?.authority ?? host,
            client_id: settings?.client_id ?? clientId,
            extraQueryParams: settings?.extraQueryParams ?? {
                audience,
            },
        }),
    );

    useEffect(() => {
        userManager.clearStaleState();
    }, [userManager]);

    const onSessionEnd = useCallback(() => {
        setIsLoggedIn(false);
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

    const onSignIn = useCallback(
        (user: User | null) => {
            setIsLoggedIn(user !== null);

            if (user !== null) {
                if (user?.state?.targetUrl !== undefined && pathname !== user?.state?.targetUrl) {
                    replace(user?.state?.targetUrl);
                } else {
                    replace(window.location.pathname); // remove code query param
                }
            }
        },
        [replace, pathname],
    );

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
            try {
                const user = await userManager.getUser();
                onSignIn(user);

                if (user === null && !isCodeCallback) await signInSilent();
            } finally {
                setInitializing(false);
            }
        };

        if (!isCodeCallback) getSession();
        else setInitializing(false);
        //eslint-disable-next-line
    }, []);

    const oidcConfig: AuthProviderProps = {
        userManager: userManager,
        autoSignIn: false,
        onSignIn,
    };

    const loading = initializing || (!isLoggedIn && isCodeCallback);
    return <AuthProvider {...oidcConfig}>{loading ? <Loading /> : children}</AuthProvider>;
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

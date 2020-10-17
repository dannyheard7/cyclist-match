import { User, UserManager, UserManagerSettings } from "oidc-client";
import React, { useCallback, useContext, useEffect, useRef, useState } from "react";

interface AuthenticationContext {
    isAuthenticated: boolean,
    user: User | null,
    loading: boolean,
    error?: Error,
    signout: () => void,
    signinSilent: () => void,
    signin: (url?: string) => void,
    signinCallback: () => void,
}

export const AuthContext = React.createContext<AuthenticationContext | undefined>(undefined);

interface Props {
    children: React.ReactNode,
    settings: UserManagerSettings
}

export const AuthenticationContextProvider: React.FC<Props> = ({ children, settings }) => {
    const userManagerRef = useRef(new UserManager(settings));
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<Error | undefined>();
    const [user, setUser] = useState<User | null>(null);
    console.log(user?.access_token);

    const setUserCallback = useCallback((user: User | null) => {
        setUser(user);
    }, [setUser]);

    const removeUserCallback = useCallback(() => {
        setUserCallback(null)
    }, [setUserCallback]);

    // We only ever want this to run once on mount (not when user manager changes because we have callbacks to handle user changes)
    useEffect(() => {
        const userManger = userManagerRef.current;

        const rehydrateUser = async () => {
            try {
                let user = await userManger.getUser();
                if (user === null) {
                    await userManger.signinSilent();
                    user = await userManger.getUser();
                }

                setError(undefined);
                setUserCallback(user);
            }
            catch (e) { setError(e) }
            finally {
                setLoading(false);
            }
        }
        rehydrateUser();

        userManger.events.addUserLoaded(setUserCallback);
        userManger.events.addUserSessionChanged(rehydrateUser);

        userManger.events.addUserSignedOut(removeUserCallback);
        userManger.events.addUserUnloaded(removeUserCallback);

        return () => {
            userManger.events.removeUserLoaded(setUserCallback);
            userManger.events.removeUserSessionChanged(rehydrateUser);
            userManger.events.removeUserSignedOut(removeUserCallback);
            userManger.events.removeUserUnloaded(removeUserCallback);
        }
        // eslint-disable-next-line
    }, []);

    const logoutCallback = useCallback(async () => {
        localStorage.clear();
        sessionStorage.clear();
        await userManagerRef.current.removeUser();
        removeUserCallback();
        await userManagerRef.current.signoutRedirect();
    }, [removeUserCallback])

    const signinSilentCallback = useCallback(async () => {
        await userManagerRef.current.signinSilentCallback();
    }, []);

    const signinCallback = useCallback(async () => {
        await userManagerRef.current.signinCallback();
    }, []);

    const signinRedirect = useCallback(async (url: string | undefined) => {
        await userManagerRef.current.signinRedirect({ state: { referrer: url } });
    }, []);

    const authentication = {
        isAuthenticated: user !== null,
        user,
        loading,
        error,
        signout: logoutCallback,
        signinSilent: signinSilentCallback,
        signin: signinRedirect,
        signinCallback
    }
    return (
        <AuthContext.Provider value={authentication}>
            {children}
        </AuthContext.Provider>
    );
}

export const useAuthentication = () => {
    const context = useContext(AuthContext)
    if (context) return context;

    throw Error("Authentication Provider was not registered.")
}

export const SignoutCallback: React.FC = () => {
    const { signout: logout } = useAuthentication();
    logout();

    return null;
}

export const SilentRenew = () => {
    const { signinSilent } = useAuthentication();
    signinSilent();

    return null;
}
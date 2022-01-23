import ky from 'ky';
import { useMemo } from 'react';
import { useAppContext } from '../Components/AppContext/AppContextProvider';
import { useAuthentication } from '../Components/Authentication/AuthWrapper';

export type HTTPError = ky.HTTPError;
export enum HTTPStatusCodes {
    Status200 = 200,
    Status401 = 401,
    Status404 = 404,
}

export const useApiCustomAuth = (bearerToken: string | undefined, onUnauthenticatedResponse?: () => void) => {
    const {
        host: { api: apiHost },
    } = useAppContext();

    const api = useMemo(() => {
        return ky.extend({
            prefixUrl: apiHost,
            retry: 0,
            hooks: {
                beforeRequest: [
                    (request) => {
                        if (bearerToken) {
                            request.headers.set('Authorization', `Bearer ${bearerToken}`);
                        }
                    },
                ],
                afterResponse: [
                    (_request, options, response) => {
                        if (response.status === HTTPStatusCodes.Status401) {
                            onUnauthenticatedResponse && onUnauthenticatedResponse();
                        }
                    },
                ],
            },
        });
    }, [bearerToken, apiHost, onUnauthenticatedResponse]);

    return api;
};

export const useApi = () => {
    const { bearerToken, signOut } = useAuthentication();
    return useApiCustomAuth(bearerToken, signOut);
};

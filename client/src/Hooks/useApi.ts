import ky from "ky";
import { useMemo } from "react";
import { useAppContext } from "../Components/AppContext/AppContextProvider";
import { useAuthentication } from "../Components/Authentication/AuthenticationContext"

export type HTTPError = ky.HTTPError;
export enum HTTPStatusCodes {
    Status200 = 200,
    Status401 = 401,
    Status404 = 404
}

export const useApi = () => {
    const { apiHost } = useAppContext();
    const { user, signout } = useAuthentication();

    const api = useMemo(() => {
        return ky.extend({
            prefixUrl: apiHost,
            retry: 0,
            hooks: {
                beforeRequest: [
                    request => {
                        if (user) {
                            request.headers.set('Authorization', `Bearer ${user.access_token}`);
                        }
                    }
                ],
                afterResponse: [
                    (_request, options, response) => {
                        if (response.status === HTTPStatusCodes.Status401) {
                            signout();
                        }
                    }
                ]

            }
        })
    }, [user, apiHost, signout]);

    return api;
}


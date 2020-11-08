import ky from "ky";
import { useMemo } from "react";
import { useAppContext } from "../Components/AppContext/AppContextProvider";
import { useAuthentication } from "../Components/Authentication/AuthenticationContext"

export const useApi = () => {
    const { apiHost } = useAppContext();
    const { user } = useAuthentication();

    const api = useMemo(() => {
        return ky.extend({
            prefixUrl: apiHost,
            hooks: {
                beforeRequest: [
                    request => {
                        if (user) {
                            request.headers.set('Authorization', `Bearer ${user.access_token}`);
                        }
                    }
                ]
            }
        })
    }, [user, apiHost]);

    return api;
}

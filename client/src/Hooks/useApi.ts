import ky from "ky";
import { useMemo } from "react";
import { useAuthentication } from "../Components/Authentication/AuthenticationContext"

export const useApi = () => {
    const { user } = useAuthentication();

    const api = useMemo(() => {
        return ky.extend({
            prefixUrl: "http://localhost:5000/",
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
    }, [user]);

    return api;
}

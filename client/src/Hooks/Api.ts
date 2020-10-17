import ky from "ky";
import { useAuthentication } from "../Components/Authentication/AuthenticationContext"

export const useApi = () => {
    const { user } = useAuthentication();

    const api = ky.extend({
        hooks: {
            beforeRequest: [
                request => {
                    if (user) {
                        request.headers.set('Authorization', `Bearer ${user.access_token}`);
                    }
                }
            ]
        }
    });

    return api;
}

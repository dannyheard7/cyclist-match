import { useQuery } from "react-query";
import { User } from "../Common/Interfaces/User";
import { useApi } from "./useApi";

const useCurrentUser = (options: PositionOptions = {}) => {
    const api = useApi();
    const { data, isLoading, error } = useQuery('fetchUser', () => api.get("auth/user").json<User>());

    return {
        user: data,
        loading: isLoading,
        error
    }
};

export default useCurrentUser;
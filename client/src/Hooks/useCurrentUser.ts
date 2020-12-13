import { QueryStatus, useQuery } from "react-query";
import { User } from "../Common/Interfaces/User";
import { useApi } from "./useApi";

const useCurrentUser = (options: PositionOptions = {}) => {
    const api = useApi();
    const { data, status, error } = useQuery('fetchUser', () => api.get("auth/user").json<User>());

    return {
        user: data,
        loading: status === QueryStatus.Loading,
        error
    }
};

export default useCurrentUser;
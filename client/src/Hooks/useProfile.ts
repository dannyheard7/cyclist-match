import { QueryStatus, useQuery } from "react-query";
import Profile from "../Common/Interfaces/Profile";
import { useApi } from "./useApi";

const useProfile = (id: string) => {
    const api = useApi();
    const { data, status, error } = useQuery('fetchUser', () => api.get(`profiles/${id}`).json<Profile>());

    return {
        profile: data,
        loading: status === QueryStatus.Loading,
        error
    }
};

export default useProfile;
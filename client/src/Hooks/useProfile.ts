import { useQuery } from "react-query";
import Profile from "../Common/Interfaces/Profile";
import { useApi } from "./useApi";

const useProfile = (id: string) => {
    const api = useApi();
    const { data, isLoading, error } = useQuery('fetchUser', () => api.get(`profiles/${id}`).json<Profile>());

    return {
        profile: data,
        loading: isLoading,
        error
    }
};

export default useProfile;
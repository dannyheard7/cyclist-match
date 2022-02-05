import { useQuery } from 'react-query';
import Profile from '../Common/Interfaces/Profile';
import { HTTPError, useApi } from './useApi';

const useCurrentUser = (load: boolean = true) => {
    const api = useApi();
    const { data, isLoading, error, refetch } = useQuery<Profile, HTTPError>(
        'fetchUser',
        () => api.get('auth/user').json<Profile>(),
        {
            enabled: load,
        },
    );

    return {
        fetch: refetch,
        user: data,
        loading: isLoading,
        error
    };
};

export default useCurrentUser;

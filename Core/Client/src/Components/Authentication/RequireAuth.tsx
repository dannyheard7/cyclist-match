import * as React from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import ErrorMessage from '../ErrorMessage/ErrorMessage';
import Loading from '../Loading/Loading';
import { useAuthentication } from './AuthWrapper';

interface RequireAuthProps {
    redirectTo?: string;
    children?: React.ReactNode;
}

export const RequireAuth: React.FC<RequireAuthProps> = ({ redirectTo = '/login', children }) => {
    const { pathname: currentLocation } = useLocation();
    const { isLoggedIn, isLoading, isError } = useAuthentication();

    if (isLoading) return <Loading />;
    if (isError) return <ErrorMessage />;

    if (isLoggedIn) {
        return <>{children}</>;
    } else {
        return (
            <Navigate
                to={{
                    pathname: redirectTo,
                }}
                state={{ targetUrl: currentLocation }}
            />
        );
    }
};

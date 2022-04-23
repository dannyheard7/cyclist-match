import * as React from 'react';
import { Navigate } from 'react-router-dom';
import ErrorMessage from '../ErrorMessage/ErrorMessage';
import Loading from '../Loading/Loading';
import { useAuthentication } from './AuthWrapper';

interface RequireNoAuthProps {
    redirectTo?: string;
    children?: React.ReactNode;
}

export const RequireNoAuth: React.FC<RequireNoAuthProps> = ({ redirectTo = '/', children }) => {
    const { isLoggedIn, isLoading, isError } = useAuthentication();

    if (isLoading) return <Loading />;
    if (isError || isLoggedIn === undefined) return <ErrorMessage />;

    if (!isLoggedIn) {
        return <>{children}</>;
    } else {
        return <Navigate to={{ pathname: redirectTo }} />;
    }
};

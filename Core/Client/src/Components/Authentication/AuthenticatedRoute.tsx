import * as React from 'react';
import { Redirect, Route, RouteProps, useLocation } from 'react-router-dom';
import ErrorMessage from '../ErrorMessage/ErrorMessage';
import Loading from '../Loading/Loading';
import { useAuthentication } from './AuthWrapper';

export const AuthenticatedRoute: React.FC<RouteProps> = (props: RouteProps) => {
    const { pathname: currentLocation } = useLocation();
    const { isLoggedIn, loading, isError } = useAuthentication();

    if (loading) return <Loading />;
    if (isError) return <ErrorMessage />;

    if (isLoggedIn) {
        return <Route {...props} />;
    } else {
        return (
            <Redirect
                to={{
                    pathname: '/login',
                    state: { targetUrl: currentLocation },
                }}
            />
        );
    }
};

import * as React from 'react';
import { Redirect, Route, RouteProps, useLocation } from 'react-router-dom';
import ErrorMessage from '../ErrorMessage/ErrorMessage';
import Loading from '../Loading/Loading';
import { useAuthentication } from './AuthWrapper';

export const UnauthenticatedRoute: React.FC<RouteProps> = (props: RouteProps) => {
    const { isLoggedIn, isLoading, isError } = useAuthentication();

    if (isLoading) return <Loading />;
    if (isError || isLoggedIn === undefined) return <ErrorMessage />;

    if (!isLoggedIn) {
        return <Route {...props} />;
    } else {
        return <Redirect to={{ pathname: '/' }} />;
    }
};

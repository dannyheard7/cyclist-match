import * as React from 'react';
import { Redirect, Route, RouteProps, useLocation } from "react-router-dom";

import Loading from '../Loading/Loading';
import { useAuthentication } from './AuthenticationContext';

export const AuthenticatedRoute: React.FC<RouteProps> = (props: RouteProps) => {
    const { pathname: currentLocation } = useLocation();
    const { isAuthenticated, loading } = useAuthentication();

    if (loading) return <Loading />;

    if (isAuthenticated) {
        return (
            <Route {...props} />
        );
    } else {
        return <Redirect to={{
            pathname: "/login",
            state: { referrer: currentLocation }
        }} />;
    }
}
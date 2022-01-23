import * as React from 'react';
import { Redirect, Route, RouteProps, useLocation } from 'react-router-dom';
import { useAuthentication } from './AuthWrapper';

export const AuthenticatedRoute: React.FC<RouteProps> = (props: RouteProps) => {
    const { pathname: currentLocation } = useLocation();
    const { isLoggedIn } = useAuthentication();

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

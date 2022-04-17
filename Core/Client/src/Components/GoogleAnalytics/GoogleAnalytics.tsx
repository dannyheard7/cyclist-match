import React, { useEffect, useState } from 'react';
import ReactGA from 'react-ga';
import { useLocation } from 'react-router-dom';
import { useAppContext } from '../AppContext/AppContextProvider';
import { useAuthentication } from '../Authentication/AuthWrapper';

const GoogleAnalytics: React.FC = () => {
    const { gaTrackingId } = useAppContext();
    const authState = useAuthentication();

    const [previous, setPrevious] = useState<{
        pathname: string;
        search: string;
    }>();
    const location = useLocation();

    const logPageChange = (pathname: string, search: string = '') => {
        const page = pathname + search;
        const { location } = window;

        const reactGaParams: ReactGA.FieldsObject = {
            page,
            location: `${location.origin}${page}`,
        };
        if (authState.isLoggedIn) {
            reactGaParams.userId = authState.oidcProfile.sub;
        }

        ReactGA.set(reactGaParams);
        ReactGA.pageview(page);

        setPrevious({
            pathname,
            search,
        });
    };

    useEffect(() => {
        if (process.env.NODE_ENV === 'production' && gaTrackingId) {
            ReactGA.initialize(gaTrackingId);
        }
    }, [gaTrackingId]);

    useEffect(() => {
        if (
            previous === undefined ||
            previous!.pathname !== location.pathname ||
            previous!.search !== location.search
        ) {
            logPageChange(location.pathname, location.search);
        }
        // eslint-disable-next-line
    }, [location, previous]);

    return null;
};

export default GoogleAnalytics;

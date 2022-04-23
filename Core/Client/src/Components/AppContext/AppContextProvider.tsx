import ky from 'ky';
import React, { useContext } from 'react';
import { useQuery } from 'react-query';
import { HTTPError } from '../../Hooks/useApi';
import ErrorMessage from '../ErrorMessage/ErrorMessage';
import Loading from '../Loading/Loading';

interface IAppContext extends Omit<ConfigResponse, 'apiHost'> {
    host: {
        api: string;
        client: string;
    };
}

interface ConfigResponse {
    apiHost: string;
    authority: {
        host: string;
        scope: string;
        clientId: string;
        extraParams: { [key: string]: any } | null;
    };
    gaTrackingId: string | null;
    recaptchaSiteKey: string;
}

export const AppContext = React.createContext<IAppContext | null>(null);

export const AppContextProvider: React.FC<{ children?: React.ReactNode }> = ({ children }) => {
    const { data, isLoading } = useQuery<ConfigResponse, HTTPError>(
        'fetchConfig',
        async () => {
            return await ky.get('/config').json<ConfigResponse>();
        },
        { enabled: true },
    );

    if (isLoading) return <Loading />;
    if (!data) return <ErrorMessage />;

    const clientHost = `${window.location.protocol}//${window.location.hostname}${
        window.location.port ? `:${window.location.port}` : ''
    }`;

    const contextValue: IAppContext = {
        ...data,
        host: {
            api: data.apiHost,
            client: clientHost,
        },
    };

    return <AppContext.Provider value={contextValue}>{children}</AppContext.Provider>;
};

export const useAppContext = () => {
    const appContext = useContext(AppContext);

    if (appContext == null) throw new Error('App context has not been registered');

    return appContext;
};

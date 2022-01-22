import React, { useContext } from "react";
import config from '../../config';

interface IAppContext {
  apiHost: string;
  recaptchaSiteKey: string;
}

export const AppContext = React.createContext<IAppContext | null>(null);

export const AppContextProvider: React.FC = ({
  children,
}) => {
  return (
    <AppContext.Provider
      value={{
        apiHost: config.API_HOST,
        recaptchaSiteKey: config.RECAPTCHA_SITE_KEY
      }}
    >
      {children}
    </AppContext.Provider>
  );
};

export const useAppContext = () => {
  const appContext = useContext(AppContext);

  if (appContext == null) throw new Error("App context has not been registered");

  return appContext;
}
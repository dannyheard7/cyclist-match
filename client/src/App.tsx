
import { ApolloClient, ApolloProvider, createHttpLink, InMemoryCache } from "@apollo/client";
import { setContext } from "@apollo/link-context";
import { makeStyles } from "@material-ui/core";
import React, { useContext } from "react";
import { BrowserRouter, Redirect, Route, Switch } from "react-router-dom";
import styles from "./App.styles";
import AppState from "./Common/Interfaces/AppState";
import AppBar from "./Components/AppBar/AppBar";
import { AppContextProvider } from "./Components/AppContext/AppContextProvider";
import { AuthenticationContext, AuthenticationContextProvider } from "./Components/Authentication/AuthenticationContextProvider";
import Dashboard from "./Components/Dashboard/Dashboard";
import Feedback from "./Components/Feedback/Feedback";
import GoogleAnalytics from "./Components/GoogleAnalytics/GoogleAnalytics";
import Loading from "./Components/Loading/Loading";
import Login from "./Components/Login/Login";
import LoginCallback from "./Components/Login/LoginCallback";
import Profile from "./Components/Profile/Profile";
import PrivacyPolicy from "./Components/PrivacyPolicy/PrivacyPolicy";
import config from './config';

const useStyles = makeStyles(styles);

const Routes: React.FC = () => {
  const { isAuthenticated, token, loading } = useContext(AuthenticationContext);

  if (loading) return <Loading />;

  const httpLink = createHttpLink({
    uri: config.GRAPHQL_HOST
  });

  const authLink = setContext((_, { headers }) => {
    return {
      headers: {
        ...headers,
        authorization: token ? `Bearer ${token}` : ""
      }
    };
  });

  const client = new ApolloClient({
    link: authLink.concat(httpLink),
    cache: new InMemoryCache()
  });

  if (isAuthenticated) {
    return (
      <ApolloProvider client={client}>
        <Switch>
          <Route exact path="/profile/:id">
            <Profile />
          </Route>
          <Route exact path="/feedback">
            <Feedback />
          </Route>
          <Route exact path="/privacy-policy">
            <PrivacyPolicy />
          </Route>
          <Route path="/">
            <Dashboard />
          </Route>
        </Switch>
      </ApolloProvider>
    );
  } else {
    return (
      <ApolloProvider client={client}>
        <Switch>
          <Route exact path="/login">
            <Login />
          </Route>
          <Route exact path="/login-callback">
            <LoginCallback />
          </Route>
          <Route exact path="/feedback">
            <Feedback />
          </Route>
          <Route exact path="/privacy-policy">
            <PrivacyPolicy />
          </Route>
          <Route>
            <Redirect to="/login" />
          </Route>
        </Switch>
      </ApolloProvider>
    );
  }
};

const Layout: React.FC = ({ children }) => {
  const classes = useStyles();

  return (
    <React.Fragment>
      <AppBar />
      <div className={classes.toolbar} />
      <div className={classes.main}>
        {children}
      </div>
    </React.Fragment>
  );
};

const onRedirectCallback = (appState: AppState) => {
  if (appState && appState.targetUrl)
    window.location.pathname = appState.targetUrl;
};

function App() {
  return (
    <AppContextProvider>
      <AuthenticationContextProvider
        domain={config.AUTH0_DOMAIN}
        clientId={config.AUTH0_CLIENT_ID!}
        redirectUri={`${window.location.protocol}//${config.CLIENT_ADDRESS}/login-callback`}
        onRedirectCallback={onRedirectCallback}
      >
        <BrowserRouter>
          <GoogleAnalytics />
          <Layout>
            <Routes />
          </Layout>
        </BrowserRouter>
      </AuthenticationContextProvider>
    </AppContextProvider>
  );
}

export default App;


import { makeStyles } from "@material-ui/core";
import React from "react";
import { BrowserRouter, Redirect, Route, Switch } from "react-router-dom";
import styles from "./App.styles";
import AppBar from "./Components/AppBar/AppBar";
import { AppContextProvider } from "./Components/AppContext/AppContextProvider";
import { AuthenticatedRoute } from "./Components/Authentication/AuthenticatedRoute";
import { AuthenticationContextProvider, SilentRenew } from "./Components/Authentication/AuthenticationContext";
import Dashboard from "./Components/Dashboard/Dashboard";
import Feedback from "./Components/Feedback/Feedback";
import Login from "./Pages/Login/Login";
import LoginCallback from "./Pages/Login/SigninCallback";
import PrivacyPolicy from "./Components/PrivacyPolicy/PrivacyPolicy";
import config from './config';
import CreateProfile from "./Pages/CreateProfile/CreateProfile";

const useStyles = makeStyles(styles);

const Routes: React.FC = () => {
  return (
    <Switch>
      <Route exact path="/login">
        <Login />
      </Route>
      <Route exact path="/oidc-signin">
        <LoginCallback />
      </Route>
      <Route exact path="/oidc-silent-renew">
        <SilentRenew />
      </Route>
      <AuthenticatedRoute exact path="/profile/create">
        <CreateProfile />
      </AuthenticatedRoute>
      {/* <AuthenticatedRoute exact path="/profile/:id">
        <Profile />
      </AuthenticatedRoute> */}
      <Route exact path="/feedback">
        <Feedback />
      </Route>
      <Route exact path="/privacy-policy">
        <PrivacyPolicy />
      </Route>
      <AuthenticatedRoute path="/">
        <Dashboard />
      </AuthenticatedRoute>
      <Route>
        <Redirect to="/login" />
      </Route>
    </Switch>
  );
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

function App() {
  return (
    <AppContextProvider>
      <AuthenticationContextProvider
        settings={{
          response_type: "token id_token",
          scope: "openid profile email",
          filterProtocolClaims: true,
          loadUserInfo: true,
          automaticSilentRenew: true,
          authority: config.AUTH0_DOMAIN,
          client_id: config.AUTH0_CLIENT_ID,
          redirect_uri: `${config.CLIENT_HOST}/oidc-signin`,
          silent_redirect_uri: `${config.CLIENT_HOST}/oidc-silent-renew`,
          post_logout_redirect_uri: `${config.CLIENT_HOST}`,
          extraQueryParams: {
            "audience": config.AUTH0_API_AUDIENCE
          }
        }}
      >
        <BrowserRouter>
          <Layout>
            <Routes />
          </Layout>
        </BrowserRouter>
      </AuthenticationContextProvider>
    </AppContextProvider>
  );
}

export default App;

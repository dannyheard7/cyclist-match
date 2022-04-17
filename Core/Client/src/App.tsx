import React from 'react';
import { BrowserRouter, Redirect, Route, Switch } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from 'react-query';
import useAppStyles from './App.styles';
import AppBar from './Components/AppBar/AppBar';
import { AppContextProvider } from './Components/AppContext/AppContextProvider';
import { AuthenticatedRoute } from './Components/Authentication/AuthenticatedRoute';
import { AuthWrapper } from './Components/Authentication/AuthWrapper';
import ConversationsList from './Components/Conversations/ConversationsList';
import Feedback from './Components/Feedback/Feedback';
import PrivacyPolicy from './Components/PrivacyPolicy/PrivacyPolicy';
import Account from './Pages/Account/Account';
import ConversationPage from './Pages/Conversation/ConversationPage';
import CreateProfile from './Pages/CreateProfile/CreateProfile';
import Dashboard from './Pages/Dashboard/Dashboard';
import Login from './Pages/Login/Login';
import { UnauthenticatedRoute } from './Components/Authentication/UnauthenticatedRoute';

const Routes: React.FC = () => {
    return (
        <Switch>
            <UnauthenticatedRoute exact path="/login">
                <Login />
            </UnauthenticatedRoute>
            <Route exact path="/feedback">
                <Feedback />
            </Route>
            <Route exact path="/privacy-policy">
                <PrivacyPolicy />
            </Route>
            <AuthenticatedRoute exact path="/profile/create">
                <CreateProfile />
            </AuthenticatedRoute>
            <AuthenticatedRoute exact path="/account">
                <Account />
            </AuthenticatedRoute>
            <AuthenticatedRoute exact path="/conversation">
                <ConversationPage />
            </AuthenticatedRoute>
            <AuthenticatedRoute exact path="/conversations">
                <ConversationsList />
            </AuthenticatedRoute>
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
    const classes = useAppStyles();

    return (
        <React.Fragment>
            <AppBar />
            <div className={classes.toolbar} />
            <div className={classes.main}>{children}</div>
        </React.Fragment>
    );
};

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            retry: false,
            refetchOnWindowFocus: false,
        },
    },
});

const App: React.FC = () => {
    return (
        <QueryClientProvider client={queryClient}>
            <AppContextProvider>
                <BrowserRouter>
                    <AuthWrapper>
                        <Layout>
                            <Routes />
                        </Layout>
                    </AuthWrapper>
                </BrowserRouter>
            </AppContextProvider>
        </QueryClientProvider>
    );
};

export default App;

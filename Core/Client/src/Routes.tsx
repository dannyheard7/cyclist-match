import React from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';
import { AuthenticatedRoute } from './Components/Authentication/AuthenticatedRoute';
import { UnauthenticatedRoute } from './Components/Authentication/UnauthenticatedRoute';
import ConversationsList from './Components/Conversations/ConversationsList';
import Feedback from './Components/Feedback/Feedback';
import PrivacyPolicy from './Components/PrivacyPolicy/PrivacyPolicy';
import Account from './Pages/Account/Account';
import ConversationPage from './Pages/Conversation/ConversationPage';
import CreateProfile from './Pages/CreateProfile/CreateProfile';
import Dashboard from './Pages/Dashboard/Dashboard';
import Login from './Pages/Login/Login';

export const Routes: React.FC = () => {
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

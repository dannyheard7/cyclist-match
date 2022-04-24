import { createTheme, StyledEngineProvider, ThemeProvider } from '@mui/material';
import React, { StrictMode } from 'react';
import { QueryClient, QueryClientProvider } from 'react-query';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { AppContextProvider } from './Components/AppContext/AppContextProvider';
import { AuthWrapper } from './Components/Authentication/AuthWrapper';
import { RequireAuth } from './Components/Authentication/RequireAuth';
import { RequireNoAuth } from './Components/Authentication/UnauthenticatedRoute';
import ConversationsList from './Components/Conversations/ConversationsList';
import Feedback from './Components/Feedback/Feedback';
import PrivacyPolicy from './Components/PrivacyPolicy/PrivacyPolicy';
import { Layout } from './Layout';
import Account from './Pages/Account/Account';
import ConversationPage from './Pages/Conversation/ConversationPage';
import CreateProfile from './Pages/CreateProfile/CreateProfile';
import Dashboard from './Pages/Dashboard/Dashboard';
import Login from './Pages/Login/Login';

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            retry: false,
            refetchOnWindowFocus: false,
        },
    },
});

const theme = createTheme();

const AppRoutes: React.FC = () => {
    return (
        <Routes>
            <Route
                path="login"
                element={
                    <RequireNoAuth>
                        <Login />
                    </RequireNoAuth>
                }
            />
            <Route path="feedback" element={<Feedback />} />
            <Route path="privacy-policy" element={<PrivacyPolicy />} />
            <Route
                path="profile/create"
                element={
                    <RequireAuth>
                        <CreateProfile />
                    </RequireAuth>
                }
            />
            <Route
                path="account"
                element={
                    <RequireAuth>
                        <Account />
                    </RequireAuth>
                }
            />
            <Route
                path="conversation"
                element={
                    <RequireAuth>
                        <ConversationPage />
                    </RequireAuth>
                }
            />
            <Route
                path="conversations"
                element={
                    <RequireAuth>
                        <ConversationsList />
                    </RequireAuth>
                }
            />
            <Route
                path="/"
                element={
                    <RequireAuth>
                        <Dashboard />
                    </RequireAuth>
                }
            />
        </Routes>
    );
};

const App: React.FC = () => {
    return (
        <StrictMode>
            <QueryClientProvider client={queryClient}>
                <AppContextProvider>
                    <BrowserRouter>
                        <AuthWrapper>
                            <StyledEngineProvider injectFirst>
                                <ThemeProvider theme={theme}>
                                    <Layout>
                                        <AppRoutes />
                                    </Layout>
                                </ThemeProvider>
                            </StyledEngineProvider>
                        </AuthWrapper>
                    </BrowserRouter>
                </AppContextProvider>
            </QueryClientProvider>
        </StrictMode>
    );
};

export default App;

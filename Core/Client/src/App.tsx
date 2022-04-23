import { createTheme, StyledEngineProvider } from '@mui/material';
import { ThemeProvider } from '@mui/styles';
import React from 'react';
import { QueryClient, QueryClientProvider } from 'react-query';
import { BrowserRouter } from 'react-router-dom';
import { AppContextProvider } from './Components/AppContext/AppContextProvider';
import { AuthWrapper } from './Components/Authentication/AuthWrapper';
import { Layout } from './Layout';
import { Routes } from './Routes';

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            retry: false,
            refetchOnWindowFocus: false,
        },
    },
});

const theme = createTheme();
const App: React.FC = () => {
    return (
        <StyledEngineProvider injectFirst>
            <ThemeProvider theme={theme}>
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
            </ThemeProvider>
        </StyledEngineProvider>
    );
};

export default App;

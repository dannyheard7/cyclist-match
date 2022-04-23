import { styled } from '@mui/material';
import React, { Fragment } from 'react';
import AppBar from './Components/AppBar/AppBar';

const Toolbar = styled('div')(({ theme }) => ({
    ...theme.mixins.toolbar,
}));

const Main = styled('div')(({ theme }) => ({
    maxWidth: theme.breakpoints.values.md,
    margin: '1rem auto',
    [theme.breakpoints.down('lg')]: {
        padding: theme.spacing(3),
    },
}));

export const Layout: React.FC<{ children?: React.ReactNode }> = ({ children }) => {
    return (
        <Fragment>
            <AppBar />
            <Toolbar />
            <Main>{children}</Main>
        </Fragment>
    );
};

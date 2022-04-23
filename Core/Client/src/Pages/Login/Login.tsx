import { Grid, Link, Typography } from '@mui/material';
import React from 'react';
import { useLocation } from 'react-router-dom';
import { useUnauthenticatedState } from '../../Components/Authentication/AuthWrapper';

const Login: React.FC = () => {
    const { signIn } = useUnauthenticatedState();
    const { state } = useLocation<{ referrer: string | undefined }>();

    return (
        <Grid container spacing={2}>
            <Grid item xs={12}>
                <Typography variant="h3" component="h1">
                    Welcome to BuddyUp
                </Typography>
            </Grid>
            <Grid item xs={12}>
                <Typography>
                    At BuddyUp you can find local cyclists to find buddy up with, whatever type of cycling you prefer
                </Typography>
            </Grid>
            <Grid item xs={12}>
                <Typography>
                    <Link href="#" onClick={() => signIn(state?.referrer)} underline="hover">
                        Sign in
                    </Link>{' '}
                    to get started
                </Typography>
            </Grid>
        </Grid>
    );
};

export default Login;

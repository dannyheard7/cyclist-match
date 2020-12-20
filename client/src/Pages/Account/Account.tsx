import { Button, Divider, Grid, Typography, useTheme } from "@material-ui/core";
import React, { useEffect } from "react";
import { useMutation } from "react-query";
import { useAuthentication } from "../../Components/Authentication/AuthenticationContext";
import ErrorMessage from "../../Components/ErrorMessage/ErrorMessage";
import Loading from "../../Components/Loading/Loading";
import { useApi } from "../../Hooks/useApi";

const Account: React.FC = () => {
    const theme = useTheme();
    const auth = useAuthentication();
    const api = useApi();

    const { mutate, isSuccess, isLoading, isError } = useMutation(() => api.delete(`auth/user`));

    useEffect(() => {
        if (isSuccess) {
            auth.signout();
        }
    }, [isSuccess, auth])

    return (
        <Grid container spacing={2}>
            <Grid container item xs={12} justify="center">
                <Typography variant="h4" component="h2">Account</Typography>
            </Grid>
            <Divider style={{ margin: theme.spacing(1, 0), width: '100%' }} />

            <Grid item xs={12}>
                <Button style={{ backgroundColor: "red" }} onClick={() => mutate()} disabled={isLoading}>Delete My Account</Button>
            </Grid>

            <Grid item xs={12}>
                {isLoading && <Loading />}
                {isError && <ErrorMessage />}
            </Grid>
        </Grid>
    )
};

export default Account;

import { Divider, Grid, Typography, useTheme } from "@material-ui/core";
import React from "react";
import { useQuery } from "react-query";
import { User } from "../../Common/Interfaces/User";
import ErrorMessage from "../../Components/ErrorMessage/ErrorMessage";
import Loading from "../../Components/Loading/Loading";
import ProfileForm from "../../Components/ProfileForm/ProfileForm";
import { useApi } from "../../Hooks/useApi";

const CreateProfile: React.FC = () => {
    const theme = useTheme();
    const api = useApi();
    const { isLoading, data, isError } = useQuery('fetchUser', () => api.get("auth/user").json<User>());

    if (isLoading) return <Loading />;
    else if (isError || !data) return <ErrorMessage />;

    return (
        <Grid container spacing={2}>
            <Grid container item xs={12} justify="center">
                <Typography variant="h4" component="h2">Create Profile</Typography>
            </Grid>
            <Divider style={{ margin: theme.spacing(1, 0), width: '100%' }} />
            <Grid item xs={12}>
                <ProfileForm
                    defaultValues={{
                        ...data
                    }}
                    onSubmit={console.log}
                />
            </Grid>
        </Grid>
    )
};

export default CreateProfile;

import { Divider, Grid, Typography, useTheme } from '@material-ui/core';
import React, { useEffect } from 'react';
import { useMutation, useQueryClient } from 'react-query';
import { useHistory } from 'react-router-dom';
import Availability from '../../Common/Enums/Availability';
import CyclingType from '../../Common/Enums/CyclingType';
import { useAuthentication } from '../../Components/Authentication/AuthWrapper';
import Loading from '../../Components/Loading/Loading';
import ProfileForm from '../../Components/ProfileForm/ProfileForm';
import { useApi } from '../../Hooks/useApi';

interface CreateProfileVariables {
    displayName: string;
    location: {
        latitude: number;
        longitude: number;
    };
    cyclingTypes: Array<CyclingType>;
    availability: Array<Availability>;
    averageDistance: number;
    averageSpeed: number;
    picture?: string;
}

const CreateProfile: React.FC = () => {
    const queryClient = useQueryClient();
    const theme = useTheme();
    const api = useApi();
    const { push } = useHistory();
    const { profile } = useAuthentication();

    const { mutate, isSuccess, isLoading } = useMutation((input: CreateProfileVariables) =>
        api.post(`profiles`, { json: input }).json<{ hasProfile: boolean }>(),
    );

    useEffect(() => {
        if (isSuccess) {
            queryClient
                .refetchQueries('fetchCurrentUser')
                .then(() => push('/'));
        }
    }, [isSuccess, push]);

    return (
        <Grid container spacing={2}>
            <Grid container item xs={12} justify="center">
                <Typography variant="h4" component="h2">
                    Create Profile
                </Typography>
            </Grid>
            <Divider style={{ margin: theme.spacing(1, 0), width: '100%' }} />
            <Grid item xs={12}>
                <ProfileForm
                    defaultValues={profile ? {
                        displayName: `${profile.given_name} ${profile.family_name}`
                    } : undefined}
                    onSubmit={mutate}
                    disabled={isLoading || isSuccess}
                />
            </Grid>
            <Grid>{isLoading && <Loading />}</Grid>
        </Grid>
    );
};

export default CreateProfile;

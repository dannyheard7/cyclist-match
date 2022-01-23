import { Divider, Grid, Typography, useTheme } from '@material-ui/core';
import React, { useEffect } from 'react';
import { useMutation } from 'react-query';
import { useHistory } from 'react-router-dom';
import Availability from '../../Common/Enums/Availability';
import CyclingType from '../../Common/Enums/CyclingType';
import ErrorMessage from '../../Components/ErrorMessage/ErrorMessage';
import Loading from '../../Components/Loading/Loading';
import ProfileForm from '../../Components/ProfileForm/ProfileForm';
import { useApi } from '../../Hooks/useApi';
import useCurrentUser from '../../Hooks/useCurrentUser';

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
    const theme = useTheme();
    const api = useApi();
    const { push } = useHistory();
    const { user, loading, error } = useCurrentUser();

    const { mutate, isSuccess, isLoading } = useMutation((input: CreateProfileVariables) =>
        api.post(`profiles`, { json: input }).json<{ hasProfile: boolean }>(),
    );

    useEffect(() => {
        if (isSuccess) push('/');
    }, [isSuccess, push]);

    if (loading) return <Loading />;
    else if (error || !user) return <ErrorMessage />;

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
                    defaultValues={{
                        ...user,
                        displayName: `${user.givenNames || ''} ${user.familyName || ''}`.trim(),
                    }}
                    onSubmit={mutate}
                />
            </Grid>
            <Grid>{isLoading && <Loading />}</Grid>
        </Grid>
    );
};

export default CreateProfile;

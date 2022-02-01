import { Card, CardContent, CardHeader, Divider, Grid, Typography, useTheme } from '@material-ui/core';
import React, { useEffect } from 'react';
import { useQuery } from 'react-query';
import { useHistory } from 'react-router-dom';
import Profile from '../../Common/Interfaces/Profile';
import { User } from '../../Common/Interfaces/User';
import { useApi } from '../../Hooks/useApi';
import useCurrentUser from '../../Hooks/useCurrentUser';
import ErrorMessage from '../ErrorMessage/ErrorMessage';
import Loading from '../Loading/Loading';

interface ProfileMatchesResponse {
    matches: Array<Profile>;
}

const ProfileMatches: React.FC = () => {
    const theme = useTheme();
    const api = useApi();
    const { push } = useHistory();

    const { user } = useCurrentUser();
    const { data, isLoading, isError, refetch } = useQuery(
        'fetchProfileMatches',
        () => api.get(`profiles/${user!.userId}/matches`).json<ProfileMatchesResponse>(),
        { enabled: false },
    );

    useEffect(() => {
        if (user) refetch();
    }, [user, refetch]);

    if (isLoading) return <Loading />;
    else if (isError || !data) return <ErrorMessage />;

    return (
        <Grid container spacing={2}>
            <Grid container item xs={12} justify="center">
                <Typography variant="h4" component="h2">
                    Top Matches
                </Typography>
            </Grid>
            <Divider style={{ margin: theme.spacing(1, 0), width: '100%' }} />
            <Grid container item xs={12} spacing={1}>
                {data.matches.map((match) => (
                    <Grid item xs={12} key={match.userId}>
                        <Card onClick={() => push(`conversations/${match.userId}`)} style={{ cursor: 'pointer' }}>
                            <CardHeader title={match.displayName} />
                            <CardContent>
                                <Typography>
                                    {match.minDistance} - {match.maxDistance}Km
                                </Typography>
                                <Typography>{match.speed}Km/H</Typography>
                                <Typography>{match.cyclingTypes.join(', ')}</Typography>
                                <Typography>{match.availability.join(', ')}</Typography>
                                <Typography>
                                    {match.locationName} - {match.distanceFromUserKM} km away
                                </Typography>
                            </CardContent>
                        </Card>
                    </Grid>
                ))}
            </Grid>
        </Grid>
    );
};

export default ProfileMatches;

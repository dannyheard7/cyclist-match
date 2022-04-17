import { Card, CardActionArea, CardContent, CardHeader, Divider, Grid, Typography, useTheme } from '@material-ui/core';
import React, { useEffect } from 'react';
import { useQuery } from 'react-query';
import { Link } from 'react-router-dom';
import Profile from '../../Common/Interfaces/Profile';
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

    const { user } = useCurrentUser();
    const { data, isLoading, isError, refetch, isIdle } = useQuery(
        'fetchProfileMatches',
        () => api.get(`matches`).json<ProfileMatchesResponse>(),
        { enabled: false },
    );

    useEffect(() => {
        if (user) refetch();
    }, [user, refetch]);

    if (isLoading || isIdle) return <Loading />;
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
                        <Card style={{ cursor: 'pointer' }}>
                            <CardActionArea component={Link} to={`conversation?userId=${match.userId}`}>
                                <CardHeader title={match.userDisplayName} />
                                <CardContent>
                                    <Typography>{match.averageDistance}Km</Typography>
                                    <Typography>{match.averageSpeed}Km/H</Typography>
                                    <Typography>{match.cyclingTypes.join(', ')}</Typography>
                                    <Typography>{match.availability.join(', ')}</Typography>
                                    <Typography>
                                        {match.locationName} - {match.distanceFromUserKM} km away
                                    </Typography>
                                </CardContent>
                            </CardActionArea>
                        </Card>
                    </Grid>
                ))}
            </Grid>
        </Grid>
    );
};

export default ProfileMatches;

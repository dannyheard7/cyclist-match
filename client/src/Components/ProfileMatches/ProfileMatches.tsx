import { Card, CardContent, CardHeader, Divider, Grid, IconButton, Typography, useTheme } from "@material-ui/core";
import SendIcon from '@material-ui/icons/Send';
import React from "react";
import { useQuery } from "react-query";
import { Link as RouterLink } from "react-router-dom";
import Profile from '../../Common/Interfaces/Profile';
import { useApi } from "../../Hooks/useApi";
import ErrorMessage from "../ErrorMessage/ErrorMessage";
import Loading from "../Loading/Loading";

interface ProfileMatchesResponse {
    matches: Array<Profile>
}

const ProfileMatches: React.FC = () => {
    const theme = useTheme();
    const api = useApi();
    const { data, isLoading, isError } = useQuery('fetchProfileMatches', () => api.get("profiles/matches").json<ProfileMatchesResponse>());

    if (isLoading) return <Loading />;
    else if (isError || !data) return <ErrorMessage />;

    return (
        <Grid container spacing={2}>
            <Grid container item xs={12} justify="center">
                <Typography variant="h4" component="h2">Top Matches</Typography>
            </Grid>
            <Divider style={{ margin: theme.spacing(1, 0), width: '100%' }} />
            <Grid container item xs={12} spacing={1}>
                {
                    data.matches.map(match => (
                        <Grid item xs={12}>
                            <Card>
                                <CardHeader title={match.displayName} />
                                <CardContent>
                                    <Typography>{match.minDistance} - {match.maxDistance}Km</Typography>
                                    <Typography>{match.speed}Km/H</Typography>
                                    <Typography>{match.cyclingTypes.join(", ")}</Typography>
                                    <Typography>{match.availability.join(", ")}</Typography>
                                    <Typography>{match.locationName} - {match.distanceFromUserKM} km away</Typography>
                                    <IconButton component={RouterLink} to={`conversations/${match.userId}`}>
                                        <SendIcon />
                                    </IconButton>
                                </CardContent>
                            </Card>
                        </Grid>
                    ))
                }
            </Grid>
        </Grid>
    )
};

export default ProfileMatches;

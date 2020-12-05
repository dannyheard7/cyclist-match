import { Card, CardContent, CardHeader, Divider, Grid, Typography, useTheme } from "@material-ui/core";
import React from "react";
import { QueryStatus, useQuery } from "react-query";
import Availability from "../../Common/Enums/Availability";
import CyclingType from "../../Common/Enums/CyclingType";
import ErrorMessage from "../ErrorMessage/ErrorMessage";
import Loading from "../Loading/Loading";
import { useApi } from "../../Hooks/useApi";

interface ProfileMatch {
    userId: string,
    displayName: string,
    locationName: string,
    cyclingTypes: Array<CyclingType>,
    availability: Array<Availability>,
    minDistance: number,
    maxDistance: number,
    speed: number,
    distanceFromUserKM: number,
    profileImage?: string
}

interface ProfileMatchesResponse {
    matches: Array<ProfileMatch>
}

const ProfileMatches: React.FC = () => {
    const theme = useTheme();
    const api = useApi();
    const { data, status } = useQuery('fetchProfileMatches', () => api.get("profiles/matches").json<ProfileMatchesResponse>());

    if (status === QueryStatus.Loading) return <Loading />;
    else if (status === QueryStatus.Error || !data) return <ErrorMessage />;

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

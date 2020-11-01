import { Divider, Grid, Typography, useTheme } from "@material-ui/core";
import React from "react";
import { QueryStatus, useMutation, useQuery } from "react-query";
import Availability from "../../Common/Enums/Availability";
import CyclingType from "../../Common/Enums/CyclingType";
import { User } from "../../Common/Interfaces/User";
import ErrorMessage from "../../Components/ErrorMessage/ErrorMessage";
import Loading from "../../Components/Loading/Loading";
import ProfileForm from "../../Components/ProfileForm/ProfileForm";
import { useApi } from "../../Hooks/useApi";

interface CreateProfileVariables {
    displayName: string,
    locationName: string,
    location: {
        latitude: number,
        longitude: number
    },
    cyclingTypes: Array<CyclingType>,
    availability: Array<Availability>,
    minDistance: number,
    maxDistance: number,
    speed: number,
    picture?: string
}

const CreateProfile: React.FC = () => {
    const theme = useTheme();
    const api = useApi();
    const { data, status } = useQuery('fetchUser', () => api.get("auth/user").json<User>());

    const [mutate, { data: mutationData, status: mutationStatus }] = useMutation((input: CreateProfileVariables) =>
        api
            .put(`profiles/${data!.id}`, { json: input })
            .json<{ hasProfile: boolean }>()
    )

    if (status === QueryStatus.Loading) return <Loading />;
    else if (status === QueryStatus.Error || !data) return <ErrorMessage />;

    return (
        <Grid container spacing={2}>
            <Grid container item xs={12} justify="center">
                <Typography variant="h4" component="h2">Create Profile</Typography>
            </Grid>
            <Divider style={{ margin: theme.spacing(1, 0), width: '100%' }} />
            <Grid item xs={12}>
                <ProfileForm
                    defaultValues={{
                        ...data,
                        displayName: `${data.givenNames || ""} ${data.familyName || ""}`.trim()
                    }}
                    onSubmit={mutate}
                />
            </Grid>
            <Grid>
                {
                    mutationStatus === QueryStatus.Loading && <Loading />
                }
            </Grid>
        </Grid>
    )
};

export default CreateProfile;

import { useQuery } from '@apollo/client';
import { Grid, Typography } from '@material-ui/core';
import React from 'react';
import { useParams } from 'react-router-dom';
import { ProfileQueryData, ProfileQueryVars, PROFILE_QUERY } from '../../GraphQL/Queries/ProfileQuery';
import Loading from '../Loading/Loading';

const Profile: React.FC = () => {
  const { id } = useParams();
  const { data, loading } = useQuery<ProfileQueryData, ProfileQueryVars>(PROFILE_QUERY, {
    variables: {
      id: id!
    }
  });


  if (loading) return <Loading />;

  const dateFormatter = new Intl.DateTimeFormat('default', {
    year: 'numeric', month: 'long', day: 'numeric', weekday: 'short', hour: 'numeric', minute: 'numeric'
  });

  const profile = data!.profile;

  return (
    <Grid container direction="column" spacing={1}>
      <Grid item><Typography variant="h4" component="h4">{profile.name}</Typography></Grid>
      <Grid item><Typography >Joined on: {dateFormatter.format(new Date(profile.createdAt))}</Typography></Grid>
      <Grid item><Typography>Location: {profile.placeName}</Typography></Grid>
    </Grid>
  );
};

export default Profile;

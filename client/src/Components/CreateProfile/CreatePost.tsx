import { useMutation } from '@apollo/client';
import { Button, FormGroup, Grid, makeStyles, TextField, Typography } from '@material-ui/core';
import React, { useEffect } from 'react';
import { ErrorMessage, useForm } from 'react-hook-form';
import { useHistory } from 'react-router-dom';
import { CreateProfileMutationData, CreateProfileMutationVars, CREATE_PROFILE_MUTATION } from '../../GraphQL/Mutations/CreateProfile';
import Loading from '../Loading/Loading';
import styles from './CreatePost.styles';

const useStyles = makeStyles(styles);

const CreateProfile: React.FC = () => {
     const classes = useStyles();
     const { push } = useHistory();
     const { register, handleSubmit, errors } = useForm();

     const [createProfile, { data, loading }] = useMutation<CreateProfileMutationData, CreateProfileMutationVars>(CREATE_PROFILE_MUTATION);

     useEffect(() => {
          if (data && data.createProfile) push(`/my/profile`);
     }, [data, push]);

     if (loading) return <Loading />;

     const onSubmit = (values: Record<string, any>) => {
          createProfile({
               variables: {
                    profile: {
                         name: values.name,
                         placeName: values.placeName,
                         exactLocationLongitude: values.placeLongitude,
                         exactLocationLatitude: values.placeLatitude,
                         preferredCyclingTypes: values.cyclingTypes
                    }
               }
          })
     };

     return (
          <form onSubmit={handleSubmit(onSubmit)}>
               <Grid container direction="column" spacing={2}>
                    <Grid item md={12}>
                         <Typography component="h1" variant="h3">Create Profile</Typography>
                    </Grid>
                    <Grid item>
                         <FormGroup>
                              <TextField
                                   multiline={true} rows={3} aria-label="Text" placeholder="Text" name="text"
                                   inputRef={register({ required: true })} error={errors.text !== undefined} />
                              <ErrorMessage name="text" message="Post text is required" errors={errors} />
                         </FormGroup>
                    </Grid>
                    <Grid item container justify="flex-end">
                         <Button type="submit" variant="contained" color="primary">Create Post</Button>
                    </Grid>
               </Grid>
          </form>
     );
};

export default CreateProfile;

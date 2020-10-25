import { Grid, Link, Typography } from "@material-ui/core";
import React from "react";
import { useLocation } from "react-router-dom";
import { useAuthentication } from "../../Components/Authentication/AuthenticationContext";

const Login: React.FC = () => {
  const { signin } = useAuthentication();
  const { state } = useLocation<{ referrer: string | undefined }>();

  return (
    <Grid container spacing={2}>
      <Grid item xs={12}>
        <Typography variant="h3" component="h1">Welcome to Elevait</Typography>
      </Grid>
      <Grid item xs={12}>
        <Typography>At Elevait you can manage your social media presence in one location, creating posts quickly and understanding their impacts with our analytics</Typography>
      </Grid>
      <Grid item xs={12}>
        <Typography>We are currently integrated with Facebook and Twitter, and will continue adding more networks</Typography>
      </Grid>
      <Grid item xs={12}>
        <Typography><Link href="#" onClick={() => signin(state.referrer)}>Sign in</Link> to get started</Typography>
      </Grid>
    </Grid>
  );
}

export default Login;

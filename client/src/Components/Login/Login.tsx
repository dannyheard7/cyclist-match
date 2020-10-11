import { Grid, Link, Typography } from "@material-ui/core";
import React from "react";
import { useAuthentication } from "../Authentication/AuthenticationContext";

const Login: React.FC = () => {
  const { login } = useAuthentication()

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
        <Typography><Link href="#" onClick={() => login()}>Log in</Link> to get started</Typography>
      </Grid>
    </Grid>
  );
}

export default Login;

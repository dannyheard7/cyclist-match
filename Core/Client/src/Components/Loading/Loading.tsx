import React from "react";
import { CircularProgress, Grid } from "@mui/material";

const Loading: React.FC = () => {
    return (
        <Grid container justifyContent="center" item md={3} >
            <CircularProgress />
        </Grid>
    );
};

export default Loading;

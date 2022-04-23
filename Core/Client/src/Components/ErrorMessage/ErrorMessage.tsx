import { Typography } from "@mui/material";
import React from "react";

interface Props {
    message?: string
}

const ErrorMessage: React.FC<Props> = ({ message }) => {
    return (
        <Typography>
            {message || "Sorry an error occured"}
        </Typography>
    );
};

export default ErrorMessage;

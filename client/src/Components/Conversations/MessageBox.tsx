import { ErrorMessage } from "@hookform/error-message";
import { zodResolver } from "@hookform/resolvers/zod";
import { Grid, IconButton, InputAdornment, TextField } from "@material-ui/core";
import SendIcon from '@material-ui/icons/Send';
import React from "react";
import { useForm } from "react-hook-form";
import * as zod from 'zod';

const schema = zod.object({
    message: zod.string().nonempty('Required'),
});

type SchemaType = zod.infer<typeof schema>;

interface Props {
    onSubmit: (values: SchemaType) => void,
    disabled?: boolean
}


const MessageBox: React.FC<Props> = ({ onSubmit: onSubmitCallback, disabled }) => {
    const resolver = zodResolver(schema);
    const { handleSubmit, register, errors, reset } = useForm<SchemaType>({ resolver });

    const onSubmit = (values: SchemaType) => {
        onSubmitCallback(values);
        reset();
    }

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <Grid container spacing={3}>
                <Grid item xs={12}>
                    <TextField
                        name="message"
                        label="Message"
                        inputRef={register()}
                        fullWidth
                        multiline
                        InputProps={{
                            endAdornment: (
                                <InputAdornment position="start">
                                    <IconButton type="submit" disabled={disabled}>
                                        <SendIcon />
                                    </IconButton>
                                </InputAdornment>),
                        }}
                        disabled={disabled}
                        error={errors.message !== undefined}
                    />
                    <ErrorMessage name="message" errors={errors} />
                </Grid>
            </Grid>
        </form>
    );
};

export default MessageBox;
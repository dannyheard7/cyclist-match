import { zodResolver } from '@hookform/resolvers/zod';
import { IconButton, InputAdornment, TextField } from '@mui/material';
import SendIcon from '@mui/icons-material/Send';
import React from 'react';
import { useForm } from 'react-hook-form';
import * as zod from 'zod';

const schema = zod.object({
    body: zod.string().nonempty('Required'),
});

type SchemaType = zod.infer<typeof schema>;

interface Props {
    onSubmit: (values: SchemaType) => void;
    disabled?: boolean;
}

const MessageBox: React.FC<Props> = ({ onSubmit: onSubmitCallback, disabled }) => {
    const resolver = zodResolver(schema);
    const {
        handleSubmit,
        register,
        formState: { errors },
        reset,
    } = useForm<SchemaType>({ resolver });

    const onSubmit = (values: SchemaType) => {
        onSubmitCallback(values);
        reset();
    };
    const onFormSubmit = handleSubmit(onSubmit);

    const onEnterPress = (e: React.KeyboardEvent) => {
        if (e.key === 'Enter' && e.shiftKey === false) {
            e.preventDefault();
            onFormSubmit();
        }
    };

    return (
        <form onSubmit={onFormSubmit}>
            <TextField
                label="Message"
                inputProps={{ ...register('body') }}
                fullWidth
                multiline
                onKeyDown={onEnterPress}
                InputProps={{
                    endAdornment: (
                        <InputAdornment position="start">
                            <IconButton type="submit" disabled={disabled} size="large">
                                <SendIcon />
                            </IconButton>
                        </InputAdornment>
                    ),
                }}
                disabled={disabled}
                error={errors.body !== undefined}
            />
        </form>
    );
};

export default MessageBox;

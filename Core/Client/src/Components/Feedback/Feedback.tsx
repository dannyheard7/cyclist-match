import { Button, FormGroup, Grid, TextField, Typography } from '@mui/material';
import React, { useRef } from 'react';
import ReCAPTCHA from 'react-google-recaptcha';
import { useForm } from 'react-hook-form';
import { ErrorMessage as FormErrorMessage } from '@hookform/error-message';
import { useMutation } from 'react-query';
import { useApi } from '../../Hooks/useApi';
import { useAppContext } from '../AppContext/AppContextProvider';
import { useAuthentication } from '../Authentication/AuthWrapper';
import ErrorMessage from '../ErrorMessage/ErrorMessage';
import Loading from '../Loading/Loading';

interface FeedbackFormValues {
    message: string;
    email?: string;
}

const Feedback: React.FC = () => {
    const { recaptchaSiteKey } = useAppContext();
    const { register, handleSubmit, errors, getValues } = useForm<FeedbackFormValues>();
    const recaptchaRef = useRef<ReCAPTCHA>(null);
    const { isLoggedIn } = useAuthentication();
    const api = useApi();

    const {
        mutate: sendFeedback,
        isSuccess,
        isLoading,
        isError,
    } = useMutation((input: FeedbackFormValues) => api.post(`feedback`, { json: input }));

    if (isLoading) return <Loading />;

    const onSubmit = () => recaptchaRef.current!.execute();

    const onCaptchaVerify = () => {
        const values = getValues();
        sendFeedback(values);
    };

    return (
        <Grid container direction="column" spacing={2}>
            <Grid item md={12}>
                <Typography variant="h2" component="h1">
                    Submit Feedback
                </Typography>
            </Grid>
            {isSuccess ? (
                <Grid item md={12}>
                    <Typography>Thank you for submitting feedback.</Typography>
                </Grid>
            ) : isError ? (
                <Grid item md={12}>
                    <ErrorMessage />
                </Grid>
            ) : (
                <form onSubmit={handleSubmit(onSubmit)}>
                    <Grid container direction="column" spacing={3}>
                        <Grid item md={12}>
                            <Typography>
                                Any feedback is appreciated, including new features you would like or bugs you have seen
                            </Typography>
                        </Grid>
                        {!isLoggedIn && (
                            <Grid item>
                                <FormGroup>
                                    <TextField
                                        multiline={true}
                                        aria-label="Email"
                                        placeholder="Email Address"
                                        name="email"
                                        inputRef={register()}
                                        error={errors.email !== undefined}
                                    />
                                </FormGroup>
                            </Grid>
                        )}
                        <Grid item>
                            <FormGroup>
                                <TextField
                                    multiline={true}
                                    rows={5}
                                    aria-label="Message"
                                    placeholder="Message"
                                    name="message"
                                    inputRef={register({ required: true })}
                                    error={errors.message !== undefined}
                                />
                                <FormErrorMessage name="message" message="Message is required" errors={errors} />
                            </FormGroup>
                        </Grid>
                        <ReCAPTCHA
                            ref={recaptchaRef}
                            size="invisible"
                            sitekey={recaptchaSiteKey}
                            onChange={onCaptchaVerify}
                        />
                        <Grid item container justifyContent="flex-end">
                            <Button type="submit" variant="contained" color="primary">
                                Submit
                            </Button>
                        </Grid>
                    </Grid>
                </form>
            )}
        </Grid>
    );
};

export default Feedback;

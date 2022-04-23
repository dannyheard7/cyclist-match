import { ErrorMessage } from '@hookform/error-message';
import { styled } from '@mui/material/styles';
import { zodResolver } from '@hookform/resolvers/zod';
import {
    Avatar,
    Button,
    Checkbox,
    FormControlLabel,
    Grid,
    IconButton,
    InputAdornment,
    TextField,
    Theme,
} from '@mui/material';
import { MyLocation as GetLocationIcon } from '@mui/icons-material';
import React, { useEffect } from 'react';
import { Controller, useForm } from 'react-hook-form';
import * as zod from 'zod';
import Availability from '../../Common/Enums/Availability';
import CyclingType from '../../Common/Enums/CyclingType';
import { useApi } from '../../Hooks/useApi';
import usePosition from '../../Hooks/usePosition';

const PREFIX = 'ProfileForm';

const classes = {
    avatar: `${PREFIX}-avatar`
};

const Root = styled('form')((
    {
        theme: Theme
    }
) => ({
    [`& .${classes.avatar}`]: {
        width: 60,
        height: 60,
    }
}));

const schema = zod.object({
    displayName: zod.string().nonempty('Required'),
    locationName: zod.string().nonempty('Required'),
    cyclingTypes: zod
        .array(
            zod.object({
                key: zod.string(),
                name: zod.string().nonempty(),
                selected: zod.boolean(),
            }),
        )
        .refine((types) => types.some((type) => type.selected), {
            message: 'You must choose at least one preferred cycling type',
        }),
    availability: zod
        .array(
            zod.object({
                key: zod.string(),
                name: zod.string().nonempty(),
                selected: zod.boolean(),
            }),
        )
        .refine((types) => types.some((type) => type.selected), {
            message: 'You must choose at least one availability',
        }),
    averageDistance: zod.number().positive(),
    averageSpeed: zod.number().positive(),
    picture: zod.string().optional(),
});

type SchemaType = zod.infer<typeof schema>;

interface ProfileFormValues {
    displayName: string;
    location: {
        name: string;
        latitude: number;
        longitude: number;
    };
    cyclingTypes: Array<CyclingType>;
    availability: Array<Availability>;
    averageDistance: number;
    averageSpeed: number;
    picture?: string;
}

interface Props {
    defaultValues?: Partial<ProfileFormValues>;
    onSubmit: (values: ProfileFormValues) => void;
    disabled?: boolean;
}

const ProfileForm: React.FC<Props> = ({ defaultValues, onSubmit: onSubmitCallback, disabled }) => {


    const resolver = zodResolver(schema);
    const { handleSubmit, register, setValue, control, errors, setError } = useForm({
        defaultValues: {
            ...defaultValues,
            cyclingTypes: Object.entries(CyclingType).map(([key, value]) => ({ key, name: value, selected: false })),
            availability: Object.entries(Availability).map(([key, value]) => ({ key, name: value, selected: false })),
        },
        resolver,
    });

    const [getPosition, { position, error: positionError }] = usePosition();
    const api = useApi();

    useEffect(() => {
        if (position) {
            api.get(`location/name?latitude=${position.latitude.toFixed(4)}&longitude=${position.longitude.toFixed(4)}`)
                .json<{ name: string }>()
                .then((res) => setValue('locationName', res.name))
                .catch(() => setError('locationName', { type: 'manual', message: 'Error fetching location' }));
        }
    }, [position, api, setError, setValue]);

    useEffect(() => {
        if (positionError) setError('locationName', { type: 'manual', message: 'Error fetching location' });
    }, [positionError, setError]);

    const onSubmit = (data: SchemaType) => {
        const { availability, cyclingTypes, ...rest } = data;

        const mappedAvailability = availability
            .filter((type) => type.selected)
            .map((type) => Availability[type.key as keyof typeof Availability]);
        const mappedCyclingTypes = cyclingTypes
            .filter((type) => type.selected)
            .map((type) => CyclingType[type.key as keyof typeof CyclingType]);

        onSubmitCallback({
            ...rest,
            availability: mappedAvailability,
            cyclingTypes: mappedCyclingTypes,
            location: {
                name: data.locationName,
                latitude: position!.latitude,
                longitude: position!.longitude,
            },
        });
    };

    return (
        <Root onSubmit={handleSubmit(onSubmit)}>
            <Grid container spacing={3}>
                <Grid container item xs={12} justifyContent="center">
                    <Avatar alt="Profile picture" src={defaultValues?.picture} className={classes.avatar} />
                </Grid>
                <Grid item xs={12}>
                    <TextField
                        name="displayName"
                        label="Display Name (Public)"
                        inputRef={register()}
                        fullWidth
                        disabled={disabled}
                    />
                    <ErrorMessage name="displayName" errors={errors} />
                </Grid>
                <Grid item xs={12}>
                    <Controller
                        name="locationName"
                        control={control}
                        as={
                            <TextField
                                disabled
                                label="Location"
                                error={errors.locationName !== undefined}
                                InputProps={{
                                    endAdornment: (
                                        <InputAdornment position="start">
                                            <IconButton onClick={() => getPosition()} disabled={disabled} size="large">
                                                <GetLocationIcon />
                                            </IconButton>
                                        </InputAdornment>
                                    ),
                                }}
                                fullWidth
                            />
                        }
                        defaultValue={''}
                    />
                    <ErrorMessage name="locationName" errors={errors} />
                </Grid>
                <Grid item xs={12}>
                    <Grid container item xs={12} justifyContent="flex-start">
                        {Object.entries(CyclingType).map(([key, name], index) => (
                            <FormControlLabel
                                key={key}
                                control={
                                    <Controller
                                        render={(props) => (
                                            <Checkbox
                                                color="primary"
                                                onChange={(e) =>
                                                    props.onChange({ ...props.value, selected: e.target.checked })
                                                }
                                                checked={props.value.selected || false}
                                                disabled={disabled}
                                            />
                                        )}
                                        control={control}
                                        name={`cyclingTypes[${index}]`}
                                        defaultValue={false}
                                    />
                                }
                                label={name}
                            />
                        ))}
                    </Grid>
                    <Grid item xs={12}>
                        <ErrorMessage name="cyclingTypes" errors={errors} />
                    </Grid>
                </Grid>
                <Grid item xs={12}>
                    <Grid container item xs={12} justifyContent="flex-start">
                        {Object.entries(Availability).map(([key, name], index) => (
                            <FormControlLabel
                                key={key}
                                control={
                                    <Controller
                                        render={(props) => (
                                            <Checkbox
                                                color="primary"
                                                onChange={(e) =>
                                                    props.onChange({ ...props.value, selected: e.target.checked })
                                                }
                                                checked={props.value.selected || false}
                                                disabled={disabled}
                                            />
                                        )}
                                        control={control}
                                        name={`availability[${index}]`}
                                        defaultValue={false}
                                    />
                                }
                                label={name}
                            />
                        ))}
                    </Grid>
                    <Grid item xs={12}>
                        <ErrorMessage name="availability" errors={errors} />
                    </Grid>
                </Grid>
                <Grid item xs={12} sm={4}>
                    <Controller
                        name="averageDistance"
                        control={control}
                        render={({ value, onChange }) => (
                            <TextField
                                onChange={(e) => onChange(parseInt(e.target.value, 10))}
                                value={value}
                                error={errors.averageDistance !== undefined}
                                label="Average Distance (Km)"
                                type="number"
                                disabled={disabled}
                                fullWidth
                            />
                        )}
                        defaultValue={40}
                    />
                    <ErrorMessage name="averageDistance" errors={errors} />
                </Grid>
                <Grid item xs={12} sm={4}>
                    <Controller
                        name="averageSpeed"
                        control={control}
                        render={({ value, onChange }) => (
                            <TextField
                                onChange={(e) => onChange(parseInt(e.target.value, 10))}
                                value={value}
                                error={errors.averageSpeed !== undefined}
                                label="Preferred Speed (Km/H)"
                                type="number"
                                disabled={disabled}
                                fullWidth
                            />
                        )}
                        defaultValue={12}
                    />
                    <ErrorMessage name="averageSpeed" errors={errors} />
                </Grid>
                <Grid container item justifyContent="flex-end">
                    <Button type="submit" color="primary" variant="contained" disabled={disabled}>
                        Save
                    </Button>
                </Grid>
            </Grid>
        </Root>
    );
};

export default ProfileForm;

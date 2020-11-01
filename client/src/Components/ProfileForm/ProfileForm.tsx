import { ErrorMessage } from "@hookform/error-message";
import { zodResolver } from "@hookform/resolvers/zod";
import { Avatar, Button, Checkbox, FormControlLabel, Grid, IconButton, InputAdornment, makeStyles, TextField, Theme } from "@material-ui/core";
import { MyLocation as GetLocationIcon } from "@material-ui/icons";
import React, { useEffect } from "react";
import { Controller, useForm } from "react-hook-form";
import * as zod from 'zod';
import Availability from "../../Common/Enums/Availability";
import CyclingType from "../../Common/Enums/CyclingType";
import { useApi } from "../../Hooks/useApi";
import usePosition from "../../Hooks/usePosition";

const useStyles = makeStyles((theme: Theme) => ({
    avatar: {
        width: 60,
        height: 60
    }
}))

const schema = zod.object({
    displayName: zod.string().nonempty('Required'),
    locationName: zod.string().nonempty('Required'),
    cyclingTypes: zod.array(
        zod.object({
            key: zod.string(),
            name: zod.string().nonempty(),
            selected: zod.boolean()
        })
    ).refine(types => types.some(type => type.selected), {
        message: "You must choose at least one preferred cycling type",
    }),
    availability: zod.array(
        zod.object({
            key: zod.string(),
            name: zod.string().nonempty(),
            selected: zod.boolean()
        })
    ).refine(types => types.some(type => type.selected), {
        message: "You must choose at least one availability",
    }),
    minDistance: zod.number().positive(),
    maxDistance: zod.number().positive(),
    speed: zod.number(),
    picture: zod.string().optional()
}).refine(values => values.maxDistance > values.minDistance, {
    message: "Maximum distance must be greater than minimum distance"
});

type SchemaType = zod.infer<typeof schema>;

interface Props {
    defaultValues?: {
        displayName: string,
        locationName?: string
        cyclingTypes?: Array<CyclingType>,
        availability?: Array<Availability>,
        minDistance?: number,
        maxDistance?: number,
        speed?: number,
        picture?: string
    },
    onSubmit: (values: {
        displayName: string,
        locationName: string,
        location: {
            latitude: number,
            longitude: number
        },
        cyclingTypes: Array<CyclingType>,
        availability: Array<Availability>,
        minDistance: number,
        maxDistance: number,
        speed: number,
        picture?: string
    }) => void,
    disabled?: boolean
}


const ProfileForm: React.FC<Props> = ({ defaultValues, onSubmit: onSubmitCallback, disabled }) => {
    const classes = useStyles();

    const resolver = zodResolver(schema);
    const { handleSubmit, register, setValue, control, errors, setError } = useForm({
        defaultValues: {
            ...defaultValues,
            cyclingTypes: Object.entries(CyclingType).map(([key, value]) => ({ key, name: value, selected: false })),
            availability: Object.entries(Availability).map(([key, value]) => ({ key, name: value, selected: false }))
        },
        resolver
    });

    const [getPosition, { position, error: positionError }] = usePosition();
    const api = useApi();

    useEffect(() => {
        if (position) {
            api
                .get(`location/name?latitude=${position.latitude.toFixed(4)}&longitude=${position.longitude.toFixed(4)}`)
                .json<{ name: string }>()
                .then(res => setValue("locationName", res.name))
                .catch(() => setError("locationName", { type: "manual", message: "Error fetching location" }))
        }
    }, [position, api, setError, setValue]);

    useEffect(() => {
        if (positionError) setError("locationName", { type: "manual", message: "Error fetching location" });
    }, [positionError, setError]);

    const onSubmit = (data: SchemaType) => {
        console.log("here");
        const { availability, cyclingTypes, ...rest } = data;

        const mappedAvailability = availability.filter(type => type.selected).map(type => Availability[type.key as keyof typeof Availability]);
        const mappedCyclingTypes = cyclingTypes.filter(type => type.selected).map(type => CyclingType[type.key as keyof typeof CyclingType]);

        console.log(mappedAvailability);
        console.log(mappedCyclingTypes);

        onSubmitCallback({
            ...rest,
            availability: mappedAvailability,
            cyclingTypes: mappedCyclingTypes,
            location: {
                latitude: position!.latitude,
                longitude: position!.longitude
            }
        })
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <Grid container spacing={3}>
                <Grid container item xs={12} justify="center">
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
                                            <IconButton onClick={() => getPosition()} disabled={disabled}>
                                                <GetLocationIcon />
                                            </IconButton>
                                        </InputAdornment>),
                                }}
                                fullWidth
                            />
                        }
                        defaultValue={""}
                    />
                    <ErrorMessage name="locationName" errors={errors} />
                </Grid>
                <Grid item xs={12}>
                    <Grid container item xs={12} justify="flex-start">
                        {
                            Object.entries(CyclingType).map(([key, name], index) => (
                                <FormControlLabel
                                    key={key}
                                    control={
                                        <Controller
                                            render={(props) => (
                                                <Checkbox
                                                    color="primary"
                                                    onChange={(e) => props.onChange({ ...props.value, selected: e.target.checked })}
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
                            ))
                        }
                    </Grid>
                    <Grid item xs={12}>
                        <ErrorMessage name="cyclingTypes" errors={errors} />
                    </Grid>
                </Grid>
                <Grid item xs={12}>
                    <Grid container item xs={12} justify="flex-start">
                        {
                            Object.entries(Availability).map(([key, name], index) => (
                                <FormControlLabel
                                    key={key}
                                    control={
                                        <Controller
                                            render={(props) => (
                                                <Checkbox
                                                    color="primary"
                                                    onChange={(e) => props.onChange({ ...props.value, selected: e.target.checked })}
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
                            ))
                        }
                    </Grid>
                    <Grid item xs={12}>
                        <ErrorMessage name="availability" errors={errors} />
                    </Grid>
                </Grid>
                <Grid item xs={12} sm={4}>
                    <Controller
                        name="minDistance"
                        control={control}
                        render={({ value, onChange }) => (
                            <TextField
                                onChange={(e) => onChange(parseInt(e.target.value, 10))}
                                value={value}
                                error={errors.minDistance !== undefined}
                                label="Minumum Distance (Km)"
                                type="number"
                                disabled={disabled}
                                fullWidth />
                        )}
                        defaultValue={5}
                    />
                    <ErrorMessage name="minDistance" errors={errors} />
                </Grid>
                <Grid item xs={12} sm={4}>
                    <Controller
                        name="maxDistance"
                        control={control}
                        render={({ value, onChange }) => (
                            <TextField
                                onChange={(e) => onChange(parseInt(e.target.value, 10))}
                                value={value}
                                error={errors.minDistance !== undefined}
                                label="Maximum Distance (Km)"
                                type="number"
                                disabled={disabled}
                                fullWidth />
                        )}
                        defaultValue={20}
                    />
                    <ErrorMessage name="maxDistance" errors={errors} />
                </Grid>
                <Grid item xs={12} sm={4}>
                    <Controller
                        name="speed"
                        control={control}
                        render={({ value, onChange }) => (
                            <TextField
                                onChange={(e) => onChange(parseInt(e.target.value, 10))}
                                value={value}
                                error={errors.speed !== undefined}
                                label="Preferred Speed (Km/H)"
                                type="number"
                                disabled={disabled}
                                fullWidth />
                        )}
                        defaultValue={12}
                    />
                    <ErrorMessage name="speed" errors={errors} />
                </Grid>
                <Grid container item justify="flex-end">
                    <Button type="submit" color="primary" variant="contained" disabled={disabled}>Save</Button>
                </Grid>
            </Grid>
        </form>
    );
};

export default ProfileForm;
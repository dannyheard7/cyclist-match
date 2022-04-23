import { useCallback, useState } from 'react';

const usePosition = (options: PositionOptions = {}) => {
    const [position, setPosition] = useState<GeolocationCoordinates>();
    const [error, setError] = useState<string>();

    const handleError = (error: GeolocationPositionError) => {
        setError(error.message);
    };

    const getPosition = useCallback(() => {
        const { geolocation } = navigator;

        if (!geolocation) {
            setError('Geolocation is not supported.');
            return;
        }

        // Call Geolocation API
        geolocation.getCurrentPosition((pos) => setPosition(pos.coords), handleError, options);
    }, [options]);

    return [getPosition, { position, error }] as const;
};

export default usePosition;

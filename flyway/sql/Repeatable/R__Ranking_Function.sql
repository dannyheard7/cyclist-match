CREATE OR REPLACE FUNCTION location_diff(location1 geography, location2 geography) RETURNS float AS $$
    SELECT  1 / GREATEST(ST_Distance(location1, location2), 0.1) 
$$ LANGUAGE SQL IMMUTABLE;


CREATE OR REPLACE FUNCTION availability_diff(
    availability1 VARCHAR(30) [],
    availability2 VARCHAR(30) []
) RETURNS integer AS $$
SELECT
    cardinality(
        array(
            select
                unnest(availability1)
            intersect
            select
                unnest(availability2)
        )
    );
$$ LANGUAGE SQL IMMUTABLE;

CREATE OR REPLACE FUNCTION speed_diff(speed1 int, speed2 int) RETURNS float AS $$
SELECT
    1 / GREATEST(abs(speed1 - speed2), 0.1)
$$ LANGUAGE SQL IMMUTABLE;

CREATE OR REPLACE FUNCTION distance_diff(
    min_distance1 int,
    max_distance1 int,
    min_distance2 int,
    max_distance2 int
) RETURNS float AS $$
SELECT
    1 / GREATEST(
        abs(
            (max_distance1 - min_distance1) - (max_distance2 - min_distance2)
        ),
        0.1
    ) $$ LANGUAGE SQL IMMUTABLE;

CREATE OR REPLACE FUNCTION ranking(
    location1 geography,
    availability1 VARCHAR(30) [],
    speed1 int,
    min_distance1 int,
    max_distance1 int,
    location2 geography,
    availability2 VARCHAR(30) [],
    speed2 int,
    min_distance2 int,
    max_distance2 int
) RETURNS integer AS $$
SELECT
    location_diff(location1, location2) * 2 + 
    availability_diff(availability1, availability2) +
    speed_diff(speed1, speed2) + 
    distance_diff(min_distance1, max_distance1, min_distance2, max_distance2)
$$ LANGUAGE SQL IMMUTABLE;


CREATE OR REPLACE FUNCTION get_profiles_by_rank(_user_id uuid) RETURNS table (
    user_id uuid,
    display_name TEXT,
    location_name TEXT
) AS $$
    WITH u as (
        SELECT
            *
        FROM
            user_profile
        WHERE
            user_id = _user_id
    )
    SELECT
        up.user_id,
        up.display_name,
        up.location_name
    FROM
        user_profile up,
        u
    WHERE up.user_id <>u.user_id
    ORDER BY
    ranking(
        up.location,
        up.availability,
        up.speed,
        up.min_distance,
        up.max_distance,
        u.location,
        u.availability,
        u.speed,
        u.min_distance,
        u.max_distance
    );
$$ LANGUAGE SQL IMMUTABLE;
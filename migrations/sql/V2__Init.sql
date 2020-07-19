CREATE EXTENSION IF NOT EXISTS postgis;

CREATE TABLE user_profile (
    user_id TEXT PRIMARY KEY,
    exact_location geography NOT NULL,
    place_name TEXT NOT NULL,
    name TEXT NOT NULL,
    preferred_cycling_types INT[] NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT now(),
    updated_at TIMESTAMP NOT NULL DEFAULT now()
);

create index on user_profile using gist (exact_location);
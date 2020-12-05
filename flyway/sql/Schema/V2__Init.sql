CREATE EXTENSION IF NOT EXISTS postgis;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE "user" (
   id UUID NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
   external_id VARCHAR(255) NOT NULL UNIQUE,
   given_names TEXT NOT NULL,
   family_name TEXT NOT NULL,
   email TEXT NOT NULL UNIQUE,
   picture TEXT
);

CREATE TABLE user_profile (
    user_id UUID NOT NULL PRIMARY KEY  REFERENCES "user" (id) ON DELETE CASCADE,
    display_name TEXT NOT NULL,
    location geography NOT NULL,
    location_name TEXT NOT NULL,
    cycling_types VARCHAR(30)[] NOT NULL,
    availability VARCHAR(30)[] NOT NULL,
    min_distance INT NOT NULL,
    max_distance INT NOT NULL,
    speed INT NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT now(),
    updated_at TIMESTAMP NOT NULL DEFAULT now()
);

create index on user_profile using gist (location);
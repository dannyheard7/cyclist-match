CREATE EXTENSION IF NOT EXISTS postgis;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE "user" (
   id UUID NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
   external_id VARCHAR(255) NOT NULL UNIQUE,
   given_name TEXT NOT NULL,
   family_name TEXT NOT NULL,
   email TEXT NOT NULL UNIQUE,
   picture TEXT
);

CREATE TABLE user_profile (
    user_id UUID NOT NULL PRIMARY KEY  REFERENCES "user" (id),
    exact_location geography NOT NULL,
    place_name TEXT NOT NULL,
    name TEXT NOT NULL,
    preferred_cycling_types INT[] NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT now(),
    updated_at TIMESTAMP NOT NULL DEFAULT now()
);

create index on user_profile using gist (exact_location);
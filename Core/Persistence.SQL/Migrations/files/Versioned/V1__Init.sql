CREATE EXTENSION IF NOT EXISTS postgis;
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE "user" (
   id UUID NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
   external_id VARCHAR(255) NOT NULL UNIQUE,
   picture TEXT,
   display_name TEXT NOT NULL,
   created_at TIMESTAMP NOT NULL DEFAULT now(),
   updated_at TIMESTAMP NOT NULL DEFAULT now()
);

CREATE TABLE user_cycling_profile (
    user_id UUID NOT NULL PRIMARY KEY  REFERENCES "user" (id) ON DELETE CASCADE,
    location geography NOT NULL,
    cycling_types VARCHAR(30)[] NOT NULL,
    availability VARCHAR(30)[] NOT NULL,
    average_distance INT NOT NULL,
    average_speed INT NOT NULL
);

create index on "user_cycling_profile" using gist (location);
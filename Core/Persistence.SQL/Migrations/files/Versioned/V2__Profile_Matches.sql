CREATE TABLE match (
    id UUID NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
    source_user_id UUID NOT NULL REFERENCES "user" (id) ON DELETE CASCADE,
    target_user_id UUID NOT NULL REFERENCES "user" (id) ON DELETE CASCADE,
    relevance NUMERIC(6, 5) NOT NULl
);

create unique index match_pair_index on match (source_user_id, target_user_id);
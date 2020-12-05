CREATE TABLE message (
    id UUID NOT NULL PRIMARY KEY,
    sender_user_id UUID NOT NULL REFERENCES "user" (id) ON DELETE CASCADE,
    receiver_user_id UUID NOT NULL REFERENCES "user" (id) ON DELETE CASCADE,
    text TEXT NOT NULL,
    sent_at TIMESTAMP NOT NULL DEFAULT now()
);
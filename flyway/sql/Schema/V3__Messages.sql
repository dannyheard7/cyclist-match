CREATE TABLE message (
    id UUID NOT NULL PRIMARY KEY,
    sender_user_id UUID NOT NULL REFERENCES "user" (id) ON DELETE CASCADE,
    receiver_user_id UUID NOT NULL REFERENCES "user" (id) ON DELETE CASCADE,
    receiver_read boolean NOT NULL DEFAULT false,
    text TEXT NOT NULL,
    sent_at TIMESTAMP NOT NULL DEFAULT now()
);

create index on message(receiver_user_id);
create index on message(sender_user_id);
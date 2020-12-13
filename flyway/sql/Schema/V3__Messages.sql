CREATE TABLE conversation (
    id UUID NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4()
);

CREATE TABLE conversation_user (
    conversation_id UUID NOT NULL REFERENCES conversation(id) ON DELETE CASCADE,
    user_id UUID NOT NULL REFERENCES "user" (id) ON DELETE CASCADE,
    latest_message_read boolean NOT NULL DEFAULT false,
    PRIMARY KEY (conversation_id, user_id)
);

CREATE TABLE conversation_message (
    id UUID NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
    conversation_id UUID NOT NULL REFERENCES conversation(id) ON DELETE CASCADE,
    sender_user_id UUID NOT NULL REFERENCES "user" (id) ON DELETE CASCADE,
    text TEXT NOT NULL,
    sent_at TIMESTAMP NOT NULL DEFAULT now()
);

create index on conversation_message(conversation_id);
create index on conversation_message(sender_user_id);
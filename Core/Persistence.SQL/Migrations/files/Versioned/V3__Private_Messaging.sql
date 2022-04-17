CREATE TABLE conversation(
    id UUID NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4()
);

CREATE TABLE message(
    id UUID NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
    body TEXT NOT NULL,
    sent_at TIMESTAMP WITH TIME ZONE NOT NULL,
    sender_id UUID NOT NULL REFERENCES "user"(id),
    conversation_id UUID NOT NULL REFERENCES conversation(id) DEFERRABLE INITIALLY DEFERRED
);

CREATE TABLE message_recipient(
    id UUID NOT NULL PRIMARY KEY DEFAULT uuid_generate_v4(),
    recipient_id UUID NOT NULL REFERENCES "user"(id) ON DELETE CASCADE,
    message_id UUID NOT NULL REFERENCES message(id) ON DELETE CASCADE DEFERRABLE INITIALLY DEFERRED,
    read_at TIMESTAMP WITH TIME ZONE DEFAULT NULL,
    read BOOLEAN GENERATED ALWAYS AS (read_at IS NOT NULL) STORED
);

create unique index message_recipient_unique_indx on message_recipient (recipient_id, message_id);
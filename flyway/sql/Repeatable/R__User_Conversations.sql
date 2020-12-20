DROP FUNCTION IF EXISTS user_conversations;
CREATE OR REPLACE FUNCTION user_conversations(_user_id uuid) RETURNS table (
    conversation_id uuid,
    message_id uuid,
    text TEXT,
    sent_at timestamp,
    sender_user_id UUID,
    latest_message_read boolean,
    user_id UUID,
    given_names TEXT,
    family_name TEXT,
    picture TEXT,
    email TEXT
) AS $$
    WITH ucs AS (
        SELECT c.id
        FROM conversation c
        INNER JOIN conversation_user cu on c.id = cu.conversation_id
        WHERE cu.user_id= _user_id
    )
    SELECT
    DISTINCT ON (c.id, u.id)
    c.id as conversation_id,
    cm.id,
    cm.text,
    cm.sent_at,
    cm.sender_user_id,
    cu.latest_message_read,
    cu.user_id,
    u.given_names,
    u.family_name,
    u.picture,
    u.email
    FROM ucs uc
    INNER JOIN conversation c on c.id = uc.id
    INNER JOIN conversation_user cu on c.id = cu.conversation_id
    INNER JOIN "user" u on u.id = cu.user_id
    INNER JOIN conversation_message cm on c.id = cm.conversation_id
    ORDER BY c.id, u.id, cm.sent_at DESC;
$$ LANGUAGE SQL IMMUTABLE;
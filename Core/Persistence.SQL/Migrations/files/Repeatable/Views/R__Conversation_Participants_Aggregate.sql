DROP VIEW IF EXISTS conversation_participants_aggregate;

CREATE VIEW conversation_participants_aggregate AS WITH conversation_participants AS (
    SELECT
        DISTINCT conversation_id,
        mr.recipient_id
    FROM
        message
        INNER JOIN message_recipient mr on message.id = mr.message_id
)
SELECT
    conversation_id,
    array_agg(recipient_id) as participants
FROM
    conversation_participants
GROUP BY
    conversation_id;
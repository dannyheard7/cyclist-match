DROP VIEW IF EXISTS conversation_participants;

CREATE VIEW conversation_participants AS
SELECT
    conversation_id,
    mr.recipient_id as participant
FROM
    message
    INNER JOIN message_recipient mr on message.id = mr.message_id
GROUP BY
    conversation_id,
    mr.recipient_id;
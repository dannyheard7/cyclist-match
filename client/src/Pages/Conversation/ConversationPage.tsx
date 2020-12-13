import { Divider, Typography } from "@material-ui/core";
import React, { Fragment } from "react";
import { useParams } from "react-router-dom";
import MessageBox from "../../Components/Conversations/MessageBox";
import MessageList from "../../Components/Conversations/MessageList";
import ErrorMessage from "../../Components/ErrorMessage/ErrorMessage";
import Loading from "../../Components/Loading/Loading";
import useConversation from '../../Hooks/useConversation';

const ConversationPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [{ participant, messages, loading, error }, { sendMessage }] = useConversation(id);

    if (loading) return <Loading />
    if (error || !messages || !participant) return <ErrorMessage />

    return (
        <Fragment >
            <Typography>{participant.displayName}</Typography>
            <MessageList messages={messages} />
            <Divider />
            <MessageBox disabled={loading} onSubmit={(values) => sendMessage(values)} />
        </Fragment >
    );
};

export default ConversationPage;

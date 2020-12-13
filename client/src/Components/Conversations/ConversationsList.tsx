import { Card, CardContent, CardHeader, Divider, Grid, Typography, useTheme } from "@material-ui/core";
import React from "react";
import { QueryStatus, useQuery } from "react-query";
import { useHistory } from "react-router-dom";
import Message from "../../Common/Interfaces/Message";
import Profile from "../../Common/Interfaces/Profile";
import { useApi } from "../../Hooks/useApi";
import useCurrentUser from "../../Hooks/useCurrentUser";
import ErrorMessage from "../ErrorMessage/ErrorMessage";
import Loading from "../Loading/Loading";

interface Conversation {
    userProfiles: Array<Profile>,
    lastMessage: Message
}

interface ConversationsResponse {
    conversations: Array<Conversation>
}

const ConversationsList: React.FC = () => {
    const theme = useTheme();
    const api = useApi();
    const { data, status } = useQuery('fetchConversations', () => api.get("conversations").json<ConversationsResponse>());
    const { user, loading } = useCurrentUser();
    const { push } = useHistory();

    if (status === QueryStatus.Loading || loading) return <Loading />;
    else if (!user || !data) return <ErrorMessage />;

    return (
        <Grid container spacing={2}>
            <Grid container item xs={12} justify="center">
                <Typography variant="h4" component="h2">Conversations</Typography>
            </Grid>
            <Divider style={{ margin: theme.spacing(1, 0), width: '100%' }} />
            <Grid container item xs={12} spacing={1}>
                {
                    data.conversations.map(conversation => {
                        const otherParticipant = conversation.userProfiles[0];

                        return (
                            <Grid item xs={12} >
                                <Card onClick={() => push(`/conversations/${otherParticipant.userId}`)}>
                                    <CardHeader title={otherParticipant.displayName} />
                                    <CardContent>
                                        <Typography>{conversation.lastMessage.sentAt}</Typography>
                                        <Typography>{conversation.lastMessage.text}</Typography>
                                    </CardContent>
                                </Card>
                            </Grid>
                        )
                    })
                }
            </Grid>
        </Grid >
    )
};

export default ConversationsList;
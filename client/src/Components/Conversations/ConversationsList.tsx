import { Box, Card, CardContent, CardHeader, Divider, Grid, Typography, useTheme } from "@material-ui/core";
import React, { useEffect } from "react";
import { useQuery } from "react-query";
import { useHistory } from "react-router-dom";
import { Conversation } from "../../Common/Interfaces/Conversation";
import { formatMessageTimestamp } from '../../Common/Utils';
import { HTTPError, useApi } from "../../Hooks/useApi";
import { ConversationResult, convertConversationResult } from "../../Hooks/useConversation";
import useCurrentUser from "../../Hooks/useCurrentUser";
import ErrorMessage from "../ErrorMessage/ErrorMessage";
import Loading from "../Loading/Loading";

interface ConversationsResponse {
    conversations: Array<ConversationResult>
}

const useConversations = () => {
    const api = useApi();
    const { user, loading } = useCurrentUser();

    const { data, isLoading, refetch } = useQuery<Array<Conversation>, HTTPError>(
        'fetchConversations',
        async () => {
            const covnersationResponse = await api.get("conversations").json<ConversationsResponse>();
            return covnersationResponse.conversations.map(cr => convertConversationResult(cr, user!))
        },
        { enabled: false }
    );

    useEffect(() => {
        if (user) refetch();
    }, [user, refetch]);

    return {
        conversations: data,
        loading: loading || isLoading
    }
}

const ConversationsList: React.FC = () => {
    const theme = useTheme();
    const { push } = useHistory();
    const { conversations, loading } = useConversations();

    if (loading) return <Loading />;
    else if (!conversations) return <ErrorMessage />;

    return (
        <Grid container spacing={2}>
            <Grid container item xs={12} justify="center">
                <Typography variant="h4" component="h2">Conversations</Typography>
            </Grid>
            <Divider style={{ margin: theme.spacing(1, 0), width: '100%' }} />
            <Grid container item xs={12} spacing={1}>
                {
                    conversations.map(conversation => {
                        const otherParticipant = conversation.userProfiles[0];

                        return (
                            <Grid item xs={12} key={conversation.id}>
                                <Card
                                    onClick={() => push(`/conversations/${otherParticipant.userId}`)}
                                    style={{ cursor: 'pointer' }}
                                >
                                    <CardHeader title={otherParticipant.displayName} />
                                    <CardContent>
                                        <Typography>{conversation.messages[0].text}</Typography>

                                        <Typography>
                                            <Box fontWeight="fontWeightLight" marginTop={1} fontSize={11}>
                                                {formatMessageTimestamp(conversation.messages[0].sentAt)}
                                            </Box>
                                        </Typography>
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
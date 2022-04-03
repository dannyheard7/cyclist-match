import { Box, Card, CardContent, CardHeader, Divider, Grid, Typography, useTheme } from '@material-ui/core';
import React from 'react';
import { useQuery } from 'react-query';
import { useHistory } from 'react-router-dom';
import { Conversation } from '../../Common/Interfaces/Conversation';
import { Page } from '../../Common/Interfaces/Page';
import { formatMessageTimestamp } from '../../Common/Utils';
import { HTTPError, useApi } from '../../Hooks/useApi';
import { ApiConversation, convertConversationResult } from '../../Hooks/useConversation';
import useCurrentUser from '../../Hooks/useCurrentUser';
import ErrorMessage from '../ErrorMessage/ErrorMessage';
import Loading from '../Loading/Loading';

const useConversations = () => {
    const api = useApi();
    const { data, isLoading } = useQuery<Array<Conversation>, HTTPError>('fetchConversations', async () => {
        const covnersationResponse = await api.get('conversations').json<Page<ApiConversation>>();
        return covnersationResponse.items.map(convertConversationResult);
    });

    return {
        conversations: data,
        loading: isLoading,
    };
};

const ConversationsList: React.FC = () => {
    const theme = useTheme();
    const { push } = useHistory();
    const { user } = useCurrentUser();
    const { conversations, loading } = useConversations();

    if (loading) return <Loading />;
    else if (!conversations || !user) return <ErrorMessage />;

    return (
        <Grid container spacing={2}>
            <Grid container item xs={12} justify="center">
                <Typography variant="h4" component="h2">
                    Conversations
                </Typography>
            </Grid>
            <Divider style={{ margin: theme.spacing(1, 0), width: '100%' }} />
            <Grid container item xs={12} spacing={1}>
                {conversations.map((conversation) => {
                    const otherParticipants = conversation.filterParticipants(user);
                    const participantIds = otherParticipants.map((x) => x.userId);

                    return (
                        <Grid item xs={12} key={participantIds.join('')}>
                            <Card
                                onClick={() =>
                                    push(`/conversation?${participantIds.map((id) => `userId=${id}`).join(`&`)}`)
                                }
                                style={{ cursor: 'pointer' }}
                            >
                                <CardHeader title={otherParticipants.map((x) => x.userDisplayName).join(', ')} />
                                <CardContent>
                                    <Typography>{conversation.messages[0].body}</Typography>

                                    <Typography>
                                        <Box fontWeight="fontWeightLight" marginTop={1} fontSize={11}>
                                            {formatMessageTimestamp(conversation.messages[0].sentAt)}
                                        </Box>
                                    </Typography>
                                </CardContent>
                            </Card>
                        </Grid>
                    );
                })}
            </Grid>
        </Grid>
    );
};

export default ConversationsList;

import { Box, Typography } from '@mui/material';
import { styled } from '@mui/material/styles';
import React, { useEffect, useRef } from 'react';
import Message from '../../Components/Conversations/Message';
import MessageBox from '../../Components/Conversations/MessageBox';
import ErrorMessage from '../../Components/ErrorMessage/ErrorMessage';
import Loading from '../../Components/Loading/Loading';
import useConversation from '../../Hooks/useConversation';
import useCurrentUser from '../../Hooks/useCurrentUser';
import useQueryParams from '../../Hooks/useQuery';

const PREFIX = 'ConversationPage';

const classes = {
    container: `${PREFIX}-container`,
    participant: `${PREFIX}-participant`,
    messages: `${PREFIX}-messages`,
    messageBox: `${PREFIX}-messageBox`,
};

const Root = styled('div')(({ theme }) => ({
    [`&.${classes.container}`]: {
        position: 'relative',
        maxHeight: '84vh',
        minHeight: '84vh',
        overflowY: 'scroll',
        overflowX: 'hidden',
        width: '100%',
        backgroundColor: theme.palette.background.default,
        display: 'flex',
        flexDirection: 'column',
    },

    [`& .${classes.participant}`]: {
        boxSizing: 'border-box',
        position: 'sticky',
        backgroundColor: theme.palette.background.default,
        top: 0,
        width: '100%',
        padding: theme.spacing(1),
        borderBottom: '1px solid black',
    },

    [`& .${classes.messages}`]: {
        padding: theme.spacing(1),
        flex: '1 1 0%',
        WebkitOverflowScrolling: 'touch',
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        '& > *': {
            maxWidth: '100%',
        },
    },

    [`& .${classes.messageBox}`]: {
        boxSizing: 'border-box',
        position: 'sticky',
        backgroundColor: theme.palette.background.default,
        bottom: 0,
        width: '100%',
        padding: theme.spacing(1),
    },
}));

const ConversationPage: React.FC = () => {
    const query = useQueryParams();

    const containerRef = useRef<HTMLDivElement>(null);
    const { user } = useCurrentUser();
    const [{ conversation, loading, error }, { sendMessage }] = useConversation(query.getAll('userId'));

    useEffect(() => {
        if (containerRef.current) {
            const height = containerRef.current.scrollHeight;
            containerRef.current?.scrollTo(0, height);
        }
    }, [conversation, containerRef]);

    if (loading) return <Loading />;
    if (error || !conversation || !user) return <ErrorMessage />;

    return (
        <Root className={classes.container} ref={containerRef}>
            <div className={classes.participant}>
                <Typography fontWeight="fontWeightBold" sx={{ margin: 1 }}>
                    {conversation
                        .filterParticipants(user)
                        .map((x) => x.userDisplayName)
                        .join(', ')}
                </Typography>
            </div>
            <div className={classes.messages}>
                {conversation.messages.map((message) => (
                    <Message
                        message={message}
                        display={message.isUserSender(user) ? 'sender' : 'receiver'}
                        key={message.id}
                    />
                ))}
            </div>
            <div className={classes.messageBox}>
                <MessageBox disabled={loading} onSubmit={(values) => sendMessage(values)} />
            </div>
        </Root>
    );
};

export default ConversationPage;

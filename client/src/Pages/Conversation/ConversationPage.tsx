import { Box, createStyles, makeStyles, Theme, Typography } from "@material-ui/core";
import React, { useEffect, useRef } from "react";
import { useParams } from "react-router-dom";
import Message from "../../Components/Conversations/Message";
import MessageBox from "../../Components/Conversations/MessageBox";
import ErrorMessage from "../../Components/ErrorMessage/ErrorMessage";
import Loading from "../../Components/Loading/Loading";
import useConversation from '../../Hooks/useConversation';


const useStyles = makeStyles((theme: Theme) => createStyles({
    container: {
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
    participant: {
        boxSizing: 'border-box',
        position: 'sticky',
        backgroundColor: theme.palette.background.default,
        top: 0,
        width: '100%',
        padding: theme.spacing(1),
        borderBottom: '1px solid black'
    },
    messages: {
        padding: theme.spacing(1),
        flex: '1 1 0%',
        WebkitOverflowScrolling: 'touch',
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        '& > *': {
            maxWidth: '100%',
        }
    },
    messageBox: {
        boxSizing: 'border-box',
        position: 'sticky',
        backgroundColor: theme.palette.background.default,
        bottom: 0,
        width: '100%',
        padding: theme.spacing(1)
    },
}));

const ConversationPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const classes = useStyles();
    const containerRef = useRef<HTMLDivElement>(null);
    const [{ participant, messages, loading, error }, { sendMessage }] = useConversation(id);

    useEffect(() => {
        if (containerRef.current) {
            const height = containerRef.current.scrollHeight;
            containerRef.current?.scrollTo(0, height);
        }

    }, [messages, containerRef])

    if (loading) return <Loading />;
    if (error || !messages || !participant) return <ErrorMessage />;

    return (
        <div className={classes.container} ref={containerRef}>
            <div className={classes.participant}>
                <Typography>
                    <Box fontWeight="fontWeightBold" m={1}>
                        {participant.displayName}
                    </Box>
                </Typography>
            </div>
            <div className={classes.messages} >
                {
                    messages.map(message => (<Message message={message} key={message.id} />))
                }
            </div>
            <div className={classes.messageBox}>
                <MessageBox disabled={loading} onSubmit={(values) => sendMessage(values)} />
            </div>
        </div>
    );
};

export default ConversationPage;
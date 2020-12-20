import { Box, Grow, Typography } from '@material-ui/core';
import React from 'react';
import Message from '../../Common/Interfaces/Message';
import { formatMessageTimestamp } from '../../Common/Utils';


const MessageComponent: React.FC<{ message: Message }> = ({ message }) => {
    const paddingLeft = message.currentUserIsSender ? '20%' : 0;
    const paddingRight = message.currentUserIsSender ? 0 : '20%';
    const bgcolor = message.currentUserIsSender ? 'primary.main' : 'background.paper';
    const color = message.currentUserIsSender ? 'primary.contrastText' : 'text.primary';
    const justifyContent = message.currentUserIsSender ? 'flex-end' : 'flex-start';

    return (
        <Grow in>
            <Box
                flex="0 0 auto"
                marginY={1}
                paddingLeft={paddingLeft}
                paddingRight={paddingRight}
                display="flex"
                justifyContent={justifyContent}
            >
                <Box
                    minWidth={0}
                    paddingY={1}
                    paddingX={2}
                    bgcolor={bgcolor}
                    color={color}
                    borderRadius={16}
                    boxShadow={2}
                >
                    <Typography style={{ overflowWrap: 'break-word', whiteSpace: 'pre-wrap' }} >
                        {message.text}
                    </Typography>
                    <Typography>
                        <Box fontWeight="fontWeightLight" marginTop={1} fontSize={11}>
                            {formatMessageTimestamp(message.sentAt)}
                        </Box>
                    </Typography>
                </Box>
            </Box>
        </Grow>
    );
}

export default MessageComponent;
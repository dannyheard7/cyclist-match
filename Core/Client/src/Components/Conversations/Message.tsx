import { Box, Grow, Typography } from '@mui/material';
import React from 'react';
import { Message } from '../../Common/Interfaces/Message';
import { formatMessageTimestamp } from '../../Common/Utils';

const MessageComponent: React.FC<{ message: Message; display: 'sender' | 'receiver' }> = ({ message, display }) => {
    const paddingLeft = display === 'sender' ? '20%' : 0;
    const paddingRight = display === 'sender' ? 0 : '20%';
    const bgcolor = display === 'sender' ? 'primary.main' : 'background.paper';
    const color = display === 'sender' ? 'primary.contrastText' : 'text.primary';
    const justifyContent = display === 'sender' ? 'flex-end' : 'flex-start';

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
                    borderRadius="16px"
                    boxShadow={2}
                >
                    <Typography style={{ overflowWrap: 'break-word', whiteSpace: 'pre-wrap' }}>
                        {message.body}
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
};

export default MessageComponent;

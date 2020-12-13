import { List, ListItem, ListItemText } from "@material-ui/core";
import React from "react";
import Message from "../../Common/Interfaces/Message";


interface Props {
    messages: Array<Message>
}

const MessageList: React.FC<Props> = ({ messages }) => {
    return (
        <List >
            {messages.map(message => (
                <ListItem>
                    <ListItemText
                        primary={message.text}
                        secondary={message.sentAt}
                    />
                </ListItem>
            ))}
        </List>
    )
};

export default MessageList;
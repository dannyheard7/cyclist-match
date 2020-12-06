import Badge from '@material-ui/core/Badge';
import MailIcon from '@material-ui/icons/Mail';
import React from "react";
import { useQuery } from "react-query";
import { useApi } from "../../Hooks/useApi";

const ConversationsIcon: React.FC = () => {
    const api = useApi();
    const { data } = useQuery('fetchNumberUnreadConversations', () => api.get("conversations/unread/count").json<number>());

    if (data) {
        return (
            <Badge badgeContent={data} color="primary">
                <MailIcon />
            </Badge>
        )
    }

    return <MailIcon />

};

export default ConversationsIcon;

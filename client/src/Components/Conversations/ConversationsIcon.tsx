import Badge from '@material-ui/core/Badge';
import MailIcon from '@material-ui/icons/Mail';
import React from "react";
import { useQuery } from "react-query";
import { Query } from '../../Common/Queries';
import { useApi } from "../../Hooks/useApi";

const ConversationsIcon: React.FC = () => {
    const api = useApi();
    const { data } = useQuery(Query.NUMBER_UNREAD_CONVERSATIONS, () => api.get("conversations/unread/count").json<number>());

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

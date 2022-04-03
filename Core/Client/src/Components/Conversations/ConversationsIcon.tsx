import Badge from '@material-ui/core/Badge';
import MailIcon from '@material-ui/icons/Mail';
import React from 'react';
import { useQuery } from 'react-query';
import { Page } from '../../Common/Interfaces/Page';
import { Query } from '../../Common/Queries';
import { useApi } from '../../Hooks/useApi';
import { ApiConversation } from '../../Hooks/useConversation';

const ConversationsIcon: React.FC = () => {
    const api = useApi();
    const { data } = useQuery(Query.NUMBER_UNREAD_CONVERSATIONS, () =>
        api.get('conversations?unread=true&pageSize=0').json<Page<ApiConversation>>(),
    );

    if (data) {
        return (
            <Badge badgeContent={data.totalCount} color="primary">
                <MailIcon />
            </Badge>
        );
    }

    return <MailIcon />;
};

export default ConversationsIcon;

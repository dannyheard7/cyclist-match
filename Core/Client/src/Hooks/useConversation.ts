import { useEffect } from 'react';
import { useMutation, useQuery, useQueryClient } from 'react-query';
import { Conversation } from '../Common/Interfaces/Conversation';
import { Message } from '../Common/Interfaces/Message';
import Profile from '../Common/Interfaces/Profile';
import { Query } from '../Common/Queries';
import { HTTPError, useApi } from './useApi';
import useCurrentUser from './useCurrentUser';

interface ApiMessage {
    id: string;
    read: boolean;
    body: string;
    sentAt: string;
    sender: Profile;
}
export interface ApiConversation {
    participants: Array<Profile>;
    messages: Array<ApiMessage>;
}

export function convertConversationResult(conversationResult: ApiConversation): Conversation {
    return new Conversation(
        conversationResult.participants,
        conversationResult.messages.map(
            (message) => new Message(message.id, message.read, message.body, new Date(message.sentAt), message.sender),
        ),
    );
}

interface SendMessageInput {
    body: string;
}

type UseConversationHook = [
    { conversation: Conversation | undefined; loading: boolean; error: any },
    { sendMessage: (values: SendMessageInput) => void },
];

const useConversation = (userIds: string[]): UseConversationHook => {
    const api = useApi();
    const queryCache = useQueryClient();
    const { user } = useCurrentUser();

    const {
        data,
        isLoading: conversationLoading,
        error: getConvoError,
        refetch,
    } = useQuery<Conversation, HTTPError>(
        `getConversation-${userIds}`,
        async () => {
            var conversationResult = await api
                .get(`conversations/byUsers?${userIds.map((x) => `userId=${x}`).join('&')}`)
                .json<ApiConversation>();
            return convertConversationResult(conversationResult);
        },
        {
            enabled: false,
            onSuccess: () => {
                queryCache.invalidateQueries(Query.NUMBER_UNREAD_CONVERSATIONS, { exact: true });
            },
        },
    );

    useEffect(() => {
        if (user) refetch();
    }, [user, refetch]);

    const { mutate: sendMessage } = useMutation<Omit<ApiMessage, 'sender'>, HTTPError, SendMessageInput>(
        async (input: SendMessageInput) =>
            await api
                .post(`conversations/message`, {
                    json: { ...input, recipients: data!.filterParticipants(user!).map((x) => x.userId) },
                })
                .json<Omit<ApiMessage, 'sender'>>(),
        {
            onSuccess: (mutationData, _) => {
                queryCache.setQueryData<Conversation>(`getConversation-${userIds}`, (old) => {
                    const newMessage: Message = new Message(
                        mutationData.id,
                        mutationData.read,
                        mutationData.body,
                        new Date(mutationData.sentAt),
                        user!,
                    );

                    if (!old) {
                        throw new Error();
                    }

                    return new Conversation(old.participants, [...old.messages, newMessage]);
                });
            },
        },
    );

    return [
        {
            conversation: data,
            loading: conversationLoading,
            error: getConvoError,
        },
        {
            sendMessage,
        },
    ];
};

export default useConversation;

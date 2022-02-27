import { useEffect } from 'react';
import { useMutation, useQuery, useQueryClient } from 'react-query';
import { Conversation } from '../Common/Interfaces/Conversation';
import Message from '../Common/Interfaces/Message';
import Profile from '../Common/Interfaces/Profile';
import { User } from '../Common/Interfaces/User';
import { Query } from '../Common/Queries';
import { HTTPError, useApi } from './useApi';
import useCurrentUser from './useCurrentUser';

export interface ConversationResult {
    id: string;
    userProfiles: Array<Profile>;
    messages: Array<{
        id: string;
        senderUserId: string;
        receiverRead: boolean;
        text: string;
        sentAt: Date;
    }>;
}

export function convertConversationResult(conversationResult: ConversationResult, user: User): Conversation {
    return {
        ...conversationResult,
        messages: conversationResult.messages.map((message) => ({
            ...message,
            sentAt: new Date(message.sentAt),
            currentUserIsSender: message.senderUserId === user.userId,
        })),
    };
}

type UseConversationHook = [
    { participant: Profile | undefined; messages: Array<Message> | undefined; loading: boolean; error: any },
    { sendMessage: (values: { message: string }) => void },
];

interface SendMessageInput {
    message: string;
}

const useConversation = (userId: string): UseConversationHook => {
    const api = useApi();
    const { user } = useCurrentUser();
    const queryCache = useQueryClient();

    const {
        data,
        isLoading: conversationLoading,
        error: getConvoError,
        refetch,
    } = useQuery<Conversation, HTTPError>(
        `getConversation-${userId}`,
        async () => {
            var conversationResult = await api.get(`messages?userId=${userId}`).json<ConversationResult>();
            return convertConversationResult(conversationResult, user!);
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

    const { mutate: sendMessage } = useMutation<{}, HTTPError, SendMessageInput>(
        (input: SendMessageInput) => api.post(`conversations/${data!.id}/message`, { json: input }),
        {
            onSuccess: (_, input) => {
                queryCache.setQueryData<Conversation>(`getConversation-${userId}`, (old) => {
                    const newMessage: Message = {
                        id: Math.random().toString(),
                        text: input.message,
                        receiverRead: false,
                        sentAt: new Date(),
                        currentUserIsSender: true,
                        senderUserId: user!.userId,
                    };

                    if (old) {
                        return {
                            ...old,
                            messages: [...old.messages, newMessage],
                        };
                    }
                    return {
                        id: data!.id,
                        userProfiles: data!.userProfiles,
                        messages: [newMessage],
                    };
                });
            },
        },
    );

    return [
        {
            participant: data?.userProfiles[0],
            messages: data?.messages,
            loading: conversationLoading,
            error: getConvoError,
        },
        {
            sendMessage,
        },
    ];
};

export default useConversation;

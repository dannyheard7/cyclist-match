import { QueryStatus, useMutation, useQuery, useQueryCache } from "react-query";
import Message from "../Common/Interfaces/Message";
import Profile from "../Common/Interfaces/Profile";
import { useApi } from "./useApi";

interface ConversationResult {
    userProfiles: Array<Profile>,
    messages: Array<Message>
}

type UseConversationHook = [
    { participant: Profile | undefined, messages: Array<Message> | undefined, loading: boolean, error: any },
    { sendMessage: (values: { message: string }) => void }
]

const useConversation = (userId: string): UseConversationHook => {
    const api = useApi();
    const { data, status, error: conversationError, refetch } = useQuery(`getConversation-${userId}`, () => api.get(`conversations/${userId}`).json<ConversationResult>());
    const queryCache = useQueryCache();

    const [sendMessage] = useMutation(
        (input: { message: string }) => api.post(`conversations/${userId}`, { json: input }),
        {
            onSuccess: () => {
                // const previousConversation = queryCache.getQueryData<ConversationResult>(`getConversation-${userId}`)

                queryCache.setQueryData<ConversationResult>(`getConversation-${userId}`, old => {
                    if (old) {
                        return {
                            userProfiles: old?.userProfiles,
                            messages: old ? [...old.messages, { text: "SomeText", receiverRead: false, sentAt: new Date().toISOString() }] : []
                        }
                    }
                    return {
                        userProfiles: [],
                        messages: []
                    };
                })
            }
        }
    )

    return [
        {
            participant: data?.userProfiles[0],
            messages: data?.messages,
            loading: status === QueryStatus.Loading,
            error: conversationError
        },
        {
            sendMessage
        }
    ]
};

export default useConversation;
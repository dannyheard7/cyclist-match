import Message from "./Message";
import Profile from "./Profile";

export interface Conversation {
    id: string,
    userProfiles: Array<Profile>,
    messages: Array<Message>
}
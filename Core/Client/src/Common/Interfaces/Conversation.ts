import { Message } from "./Message";
import Profile from "./Profile";
import { User } from "./User";

export class Conversation {
    participants: Array<Profile>;
    messages: Array<Message>;

    constructor(participants: Array<Profile>, messages: Array<Message>) {
        this.participants = participants;
        this.messages = messages;
    }

    public filterParticipants = (user: User): Array<Profile> => this.participants.filter(x => x.userId !== user.userId);
}
import Profile from "./Profile";
import { User } from "./User";

export class Message {
    id: string;
    read: boolean;
    body: string;
    sentAt: Date;
    sender: Profile;

    constructor(id: string, read: boolean, body: string, sentAt: Date, sender: Profile) {
        this.id = id;
        this.read = read;
        this.body = body;
        this.sentAt = sentAt;
        this.sender = sender;
    }

    public isUserSender = (user: User) => this.sender.userId === user.userId;
}
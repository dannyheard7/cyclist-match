export default interface Message {
    id: string,
    receiverRead: boolean,
    text: string,
    sentAt: Date,
    senderUserId: string,
    currentUserIsSender: boolean
}
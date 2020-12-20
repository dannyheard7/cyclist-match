export default interface Message {
    receiverRead: boolean,
    text: string,
    sentAt: Date,
    senderUserId: string,
    currentUserIsSender: boolean
}
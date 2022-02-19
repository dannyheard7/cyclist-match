namespace ChatService.Models;

internal class MessageBundle
{
    public MessageBundle(MessageToUser message)
    {
        Message = message;
    }

    public MessageToUser Message { get;  }
}
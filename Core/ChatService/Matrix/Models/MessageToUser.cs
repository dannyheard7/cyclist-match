namespace ChatService.Models;

internal class MessageToUser
{
    public MessageToUser(string createdBy, IReadOnlyCollection<string> recepients, DateTime createdOn)
    {
        CreatedBy = createdBy;
        Recepients = recepients;
        CreatedOn = createdOn;
    }

    public string CreatedBy { get; }

    public IReadOnlyCollection<string> Recepients { get; }
    
    public DateTime CreatedOn { get; }
}
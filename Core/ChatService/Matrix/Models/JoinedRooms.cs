using System.Text.Json.Serialization;

namespace ChatService.Models;

public class JoinedRooms
{
    public JoinedRooms(IReadOnlyCollection<string> rooms)
    {
        Rooms = rooms;
    }

    [JsonPropertyName("joined_rooms")]
    public IReadOnlyCollection<string> Rooms { get; }
}
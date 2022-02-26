using System.Text.Json.Serialization;

namespace Persistence.Profile.Types
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Availability
    {
        Weekday,
        Evening,
        Weekend
    }
}
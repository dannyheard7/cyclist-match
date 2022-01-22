using System.Text.Json.Serialization;

namespace Persistence.Entity
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Availability
    {
        Weekday,
        Evening,
        Weekend
    }
}
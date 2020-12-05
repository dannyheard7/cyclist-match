using System.Text.Json.Serialization;

namespace Persistence.Entity
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AvailabilityItem
    {
        Weekday,
        Evening,
        Weekend
    }
}
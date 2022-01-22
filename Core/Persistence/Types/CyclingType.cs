using System.Text.Json.Serialization;

namespace Persistence.Entity
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CyclingType
    {
        Road,
        Mountain,
        Touring,
        CyloCross,
        BMX,
        Track
    }
}
using System.Text.Json.Serialization;

namespace Persistence.Profile.Types
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
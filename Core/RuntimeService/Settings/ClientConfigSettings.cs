using System;
using System.Text.Json.Serialization;

namespace RuntimeService.Settings;

public class ClientConfigSettings
{
    public class AuthoritySettings
    {
        public Uri Host { get; init; }
        public string Scope { get; init; }
        public string ClientId { get; init; }
        public string Audience { get; init; }
    }
    
    [JsonIgnore]
    public const string Key = "ClientConfig";

    public string ApiHost { get; init;  }
    public AuthoritySettings Authority { get; init; }
    public string RecaptchaSiteKey { get; init; }
    public string? GaTrackingId { get; init; }
}

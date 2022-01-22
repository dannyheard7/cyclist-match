using System;
using System.Text.Json.Serialization;
using Persistence;

namespace Auth
{
    internal class Auth0User : IOIDCUser
    {
        [JsonPropertyName("sub")]
        [JsonInclude]
        public string Id { get; private set; }

        [JsonPropertyName("given_name")]
        [JsonInclude]
        public string GivenNames { get; private set; }

        [JsonPropertyName("family_name")]
        [JsonInclude]
        public string FamilyName { get; private set; }

        [JsonPropertyName("picture")]
        [JsonInclude]
        public string Picture { get; private set; }

        [JsonPropertyName("email")]
        [JsonInclude]
        public string Email { get; private set; }
    }
}
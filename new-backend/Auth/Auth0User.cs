using System;
using System.Text.Json.Serialization;
using Persistence;

namespace Auth
{
    public class Auth0User : IUser
    {
        public Guid? Id => null;
        
        [JsonPropertyName("sub")]
        [JsonInclude]
        public string ExternalId { get; private set; }
        
        [JsonPropertyName("given_name")]
        [JsonInclude]
        public string GivenName { get; private set; }
        
        [JsonPropertyName("family_name")]
        [JsonInclude]
        public  string FamilyName { get;  private set; }
        
        [JsonPropertyName("picture")]
        [JsonInclude]
        public string Picture { get; private set; }
        
        [JsonPropertyName("email")]
        [JsonInclude]
        public string Email { get; private set; }
    }
}
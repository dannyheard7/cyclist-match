using System;
using System.Text.Json.Serialization;
using Persistence;

namespace Auth
{
    internal class Auth0User : IOIDCUser
    {
        public Auth0User(string id, string givenNames, string familyName, string picture, string email)
        {
            Id = id;
            GivenNames = givenNames;
            FamilyName = familyName;
            Picture = picture;
            Email = email;
        }

        [JsonPropertyName("sub")]
        public string Id { get; }

        [JsonPropertyName("given_name")]
        public string GivenNames { get; }

        [JsonPropertyName("family_name")]
        public string FamilyName { get; }

        [JsonPropertyName("picture")]
        public string Picture { get; }

        [JsonPropertyName("email")]
        public string Email { get; }
    }
}
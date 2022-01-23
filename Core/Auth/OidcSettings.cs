namespace Auth
{
    internal class OidcSettings
    {
        public const string Key = "OIDC";
        
        public string Domain { get; init; }
        public string Audience { get; init; }
        
        public string UserInfoEndpoint { get; init; }
    }
}
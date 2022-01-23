namespace Auth
{
    internal class OidcSettings
    {
        public const string Key = "OIDC";
        
        public string Host { get; init; }
        public string Audience { get; init; }
        public string UserInfoEndpoint { get; init; }
    }
}
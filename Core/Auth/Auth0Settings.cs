namespace Auth
{
    internal class Auth0Settings
    {
        public const string Key = "Auth0";
        
        public string Domain { get; set; }
        public string Audience { get; set; }
    }
}
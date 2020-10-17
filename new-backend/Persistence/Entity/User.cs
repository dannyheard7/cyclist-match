namespace Persistence
{
    public interface IUser
    {
        public string ExternalId { get; }
        
        public string GivenName { get; }
        
        public string FamilyName { get; }
        
        public string Picture { get; }
        
        public string Email { get; }
    }
}
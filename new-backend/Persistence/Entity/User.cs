using System;

namespace Persistence
{
    public interface IUser
    {
        public Guid? Id { get; }
        public string ExternalId { get; }
        
        public string GivenName { get; }
        
        public string FamilyName { get; }
        
        public string Picture { get; }
        
        public string Email { get; }
    }
}
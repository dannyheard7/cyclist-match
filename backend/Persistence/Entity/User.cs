using System;

namespace Persistence
{
    // TODO: split this into internal user/external user so we know the difference and guid doesnt need to be nullable
    public interface IUser
    {
        public Guid? Id { get; }
        public string ExternalId { get; }

        public string GivenNames { get; }

        public string FamilyName { get; }

        public string Picture { get; }

        public string Email { get; }
    }
}
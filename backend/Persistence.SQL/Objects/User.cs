using System;

namespace Persistence.SQL.Objects
{
    internal class DBUser : IUser
    {
        public Guid? Id { get; }
        public string ExternalId { get; }
        public string GivenNames { get; }
        public string FamilyName { get; }
        public string Picture { get; }
        public string Email { get; }
    }
}
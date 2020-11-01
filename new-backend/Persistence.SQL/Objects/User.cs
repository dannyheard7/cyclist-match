using System;

namespace Persistence.SQL.Objects
{
    internal class DBUser : IUser
    {
        public Guid? Id { get; }
        public string ExternalId { get; private set; }
        public string GivenNames { get; private set; }
        public string FamilyName { get; private set; }
        public string Picture { get; private set; }
        public string Email { get; private set; }
    }
}
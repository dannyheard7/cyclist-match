using System;

namespace Persistence.SQL.Objects
{
    internal class DBUser : IUser
    {
        protected DBUser() {}
        public DBUser(Guid id, string externalId, string givenNames, string familyName, string email, string? picture)
        {
            Id = id;
            ExternalId = externalId;
            GivenNames = givenNames;
            FamilyName = familyName;
            Picture = picture;
            Email = email;
        }

        public Guid? Id { get; }
        public string ExternalId { get; }
        public string GivenNames { get; }
        public string FamilyName { get; }
        public string? Picture { get; }
        public string Email { get; }
    }
}
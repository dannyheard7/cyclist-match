using System.Linq;
using System.Security.Claims;

namespace Auth
{
    public class User
    {
        public User(string id, string name, string? email = null)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public User(ClaimsPrincipal claimsPrincipal)
        {
            Id = claimsPrincipal.FindAll(ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            Name = claimsPrincipal.FindAll(ClaimTypes.Name).FirstOrDefault()?.Value ?? "Unknown";
            Email = claimsPrincipal.FindAll(ClaimTypes.Email).FirstOrDefault()?.Value;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        
        protected bool Equals(User other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
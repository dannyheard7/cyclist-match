namespace Auth;

public interface IOIDCUser
{
    public string Id { get;  }
    
    public string GivenNames { get; }
    
    public string FamilyName { get; }

    public string Picture { get; }
    
    public string Email { get; }
}
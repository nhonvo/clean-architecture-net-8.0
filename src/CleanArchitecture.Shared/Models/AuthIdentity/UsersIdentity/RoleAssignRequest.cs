namespace CleanArchitecture.Shared.Models.AuthIdentity.UsersIdentity;

public class RoleAssignRequest
{
    public string UserId { get; set; }

    public List<SelectItem> Roles { get; set; } = [];
    public List<SelectItem> Scopes { get; set; }
}

public class SelectItem
{
    public string Name { get; set; }

    public bool Selected { get; set; }
}

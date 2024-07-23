namespace CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;

public class UserViewModel
{
    public Guid UserId { get; set; }

    public string FullName { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public Status Status { get; set; }

    public string Avatar { get; set; }

    public IList<string> Roles { get; set; }
}
public class UserQueryResult
{
    public ApplicationUser users { get; set; }
    public List<string> role { get; set; }
    public string avatar { get; set; }
}

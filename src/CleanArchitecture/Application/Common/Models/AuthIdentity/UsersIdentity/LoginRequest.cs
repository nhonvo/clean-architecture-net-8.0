namespace CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;

public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}

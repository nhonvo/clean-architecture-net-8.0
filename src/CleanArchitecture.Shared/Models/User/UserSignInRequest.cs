namespace CleanArchitecture.Shared.Models.User;

public class UserSignInRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

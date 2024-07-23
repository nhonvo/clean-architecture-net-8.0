namespace CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;

public class RegisterRequest
{
    public string UserName { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }


    public string Password { get; set; }

    public string ConfirmPassword { get; set; }
}

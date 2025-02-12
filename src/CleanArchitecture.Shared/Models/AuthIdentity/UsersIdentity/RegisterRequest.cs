using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Shared.Models.AuthIdentity.UsersIdentity;

[SwaggerSchema("Represents the registration request model.")]
public class RegisterRequest
{
    [SwaggerSchema("The username for the new account.")]
    public string UserName { get; set; }

    [SwaggerSchema("The full name of the new user.")]
    public string Name { get; set; }

    [SwaggerSchema("The email address for the new account.")]
    public string Email { get; set; }

    [SwaggerSchema("The password for the new account.")]
    public string Password { get; set; }
}

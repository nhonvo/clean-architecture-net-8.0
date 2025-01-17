using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Shared.Models.AuthIdentity.UsersIdentity;

[SwaggerSchema("Represents the login request model.")]
public class LoginRequest
{
    [SwaggerSchema("The username of the user.")]
    public string UserName { get; set; }

    [SwaggerSchema("The password of the user.")]
    public string Password { get; set; }

    [SwaggerSchema("Indicates whether to remember the user for future logins.")]
    public bool RememberMe { get; set; }
}

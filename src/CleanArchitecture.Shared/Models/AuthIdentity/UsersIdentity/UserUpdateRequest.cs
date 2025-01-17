using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Shared.Models.AuthIdentity.UsersIdentity;

public class UserUpdateRequest
{
    public Guid UserId { get; set; }

    public string UserName { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Avatar { get; set; }

    public IFormFile MediaFile { get; set; }

}

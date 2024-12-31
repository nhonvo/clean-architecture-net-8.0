using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    /// <summary>
    /// The full name of the user.
    /// </summary>
    [SwaggerSchema("The full name of the user.")]
    public string Name { get; set; }

    /// <summary>
    /// The status of the user account.
    /// </summary>
    [SwaggerSchema("The status of the user account, such as Active, Inactive, or Suspended.")]
    public Status Status { get; set; }

    /// <summary>
    /// The ID of the user's avatar media, if available.
    /// </summary>
    [SwaggerSchema("The ID of the user's avatar media, if available.")]
    public int? AvatarId { get; set; }

    /// <summary>
    /// The avatar media associated with the user.
    /// </summary>
    [SwaggerSchema("The avatar media associated with the user.")]
    public Media Avatar { get; set; }

    /// <summary>
    /// The roles assigned to the user.
    /// </summary>
    [SwaggerSchema("The roles assigned to the user.")]
    public virtual ICollection<UserRoles> UserRoles { get; set; }

    /// <summary>
    /// The refresh tokens issued to the user.
    /// </summary>
    [SwaggerSchema("The refresh tokens issued to the user.")]
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}
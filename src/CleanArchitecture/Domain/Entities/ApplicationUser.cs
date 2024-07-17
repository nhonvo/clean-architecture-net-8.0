using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Name { get; set; }

    public Status Status { get; set; }

    public int? AvatarId { get; set; }

    public Media Avatar { get; set; }
    public virtual ICollection<UserRoles> UserRoles { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}


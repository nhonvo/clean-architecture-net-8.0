using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Domain.Entities;

[NotMapped]
public class RoleIdentity : IdentityRole<Guid>
{
    public virtual ICollection<UserRoles> UserRoles { get; set; }
}


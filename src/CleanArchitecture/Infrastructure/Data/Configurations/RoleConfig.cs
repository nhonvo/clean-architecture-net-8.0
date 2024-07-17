using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class RoleConfig : IEntityTypeConfiguration<RoleIdentity>
{
    public void Configure(EntityTypeBuilder<RoleIdentity> builder)
    {
        builder.ToTable("Role");

        builder.HasMany(e => e.UserRoles)
            .WithOne(e => e.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);
    }
}

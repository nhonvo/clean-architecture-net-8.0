using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

public class ForgotPasswordConfig : IEntityTypeConfiguration<ForgotPassword>
{
    public void Configure(EntityTypeBuilder<ForgotPassword> builder)
    {
        builder.ToTable("ForgotPassword");

        builder.HasKey(x => x.Id);

        builder.Property(k => k.Id).UseIdentityColumn();

        builder.Property(x => x.Email);

    }
}

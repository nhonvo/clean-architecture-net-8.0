using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data;

public static class ModelBuilderExtension
{
    public static void Seed(this ModelBuilder modelBuilder)
    {

        var roleId = new Guid("A3314BE5-4C77-4FB6-82AD-302014682A73");

        var adminId1 = new Guid("69DB714F-9576-45BA-B5B7-F00649BE01DE");
        var adminId2 = new Guid("69DB714F-9576-45BA-B5B7-F00649BE02DE");
        var adminId3 = new Guid("69DB714F-9576-45BA-B5B7-F00649BE03DE");
        var adminId4 = new Guid("69DB714F-9576-45BA-B5B7-F00649BE04DE");
        modelBuilder.Entity<RoleIdentity>().HasData(new RoleIdentity
        {
            Id = roleId,
            Name = "Admin",
            NormalizedName = "Admin",
        });
        modelBuilder.Entity<RoleIdentity>().HasData(new RoleIdentity
        {
            Id = new Guid("B4314BE5-4C77-4FB6-82AD-302014682B13"),
            Name = "Subscriber",
            NormalizedName = "Subscriber",
        });

        var hashed = new PasswordHasher<ApplicationUser>();
        modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
        {
            Id = adminId1,
            UserName = "admin1",
            NormalizedUserName = "admin1",
            Email = "admin1@gmail.com",
            NormalizedEmail = "ADMIN1@GMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = hashed.HashPassword(null, "Admin1@123"),
            SecurityStamp = string.Empty,
            Name = "Admin 1",
        });
        modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
        {
            Id = adminId2,
            UserName = "Admin2",
            NormalizedUserName = "Admin2",
            Email = "admin2@gmail.com",
            NormalizedEmail = "ADMIN2@GMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = hashed.HashPassword(null, "Admin2@123"),
            SecurityStamp = string.Empty,
            Name = "Admin 2",
        });
        modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
        {
            Id = adminId3,
            UserName = "Admin3",
            NormalizedUserName = "Admin3",
            Email = "admin3@gmail.com",
            NormalizedEmail = "ADMIN3@GMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = hashed.HashPassword(null, "Admin3@123"),
            SecurityStamp = string.Empty,
            Name = "Admin 3",
        });
        modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
        {
            Id = adminId4,
            UserName = "Admin4",
            NormalizedUserName = "Admin4",
            Email = "admin4@gmail.com",
            NormalizedEmail = "ADMIN4@GMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = hashed.HashPassword(null, "Admin4@123"),
            SecurityStamp = string.Empty,
            Name = "Admin 4",
        });

        modelBuilder.Entity<UserRoles>().HasData(new UserRoles
        {
            RoleId = roleId,
            UserId = adminId1
        });
        modelBuilder.Entity<UserRoles>().HasData(new UserRoles
        {
            RoleId = roleId,
            UserId = adminId2
        });
        modelBuilder.Entity<UserRoles>().HasData(new UserRoles
        {
            RoleId = roleId,
            UserId = adminId3
        });
        modelBuilder.Entity<UserRoles>().HasData(new UserRoles
        {
            RoleId = roleId,
            UserId = adminId4
        });
    }
}
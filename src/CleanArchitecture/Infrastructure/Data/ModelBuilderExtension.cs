using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data;

public static class ModelBuilderExtension
{
    public static void Seed(this ModelBuilder modelBuilder)
    {

        var roleId = new Guid("A3314BE5-4C77-4FB6-82AD-302014682A73");

        var adminId = new Guid("69DB714F-9576-45BA-B5B7-F00649BE01DE");

        modelBuilder.Entity<RoleIdentity>().HasData(new RoleIdentity
        {
            Id = roleId,
            Name = Role.Admin.ToString(),
            NormalizedName = Role.Admin.ToString(),
        });
        modelBuilder.Entity<RoleIdentity>().HasData(new RoleIdentity
        {
            Id = new Guid("B4314BE5-4C77-4FB6-82AD-302014682B13"),
            Name = Role.User.ToString(),
            NormalizedName = Role.User.ToString(),
        });

        var hashed = new PasswordHasher<ApplicationUser>();
        modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
        {
            Id = adminId,
            UserName = "admin",
            NormalizedUserName = "admin",
            Email = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = hashed.HashPassword(null, "P@ssw0rd"),
            SecurityStamp = string.Empty,
            Name = "Admin 1",
        });

        modelBuilder.Entity<UserRoles>().HasData(new UserRoles
        {
            RoleId = roleId,
            UserId = adminId
        });
        modelBuilder.Entity<Book>().HasData(
                    new Book
                    {
                        Id = 1,
                        Title = "C# Programming",
                        Description = "A comprehensive guide to C# programming.",
                        Price = 29.99
                    },
                    new Book
                    {
                        Id = 2,
                        Title = "ASP.NET Core Development",
                        Description = "Learn how to build web applications using ASP.NET Core.",
                        Price = 35.50
                    },
                    new Book
                    {
                        Id = 3,
                        Title = "Entity Framework Core In Action",
                        Description = "Master the Entity Framework Core ORM for .NET development.",
                        Price = 40.00
                    },
                    new Book
                    {
                        Id = 4,
                        Title = "Blazor WebAssembly: The Complete Guide",
                        Description = "Everything you need to know about building Blazor WebAssembly applications.",
                        Price = 45.99
                    },
                    new Book
                    {
                        Id = 5,
                        Title = "Design Patterns in C#",
                        Description = "Implement common design patterns in C# to improve code structure.",
                        Price = 50.00
                    }
                );
    }
}

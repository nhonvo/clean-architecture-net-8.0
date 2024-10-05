using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Web.Extensions;
/// <summary>
/// Seeding data by ModelBuilder
/// </summary>
public static class ModelBuilderExtension
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
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
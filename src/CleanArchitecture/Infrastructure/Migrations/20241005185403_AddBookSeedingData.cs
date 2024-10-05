using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookSeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Book",
                columns: new[] { "Id", "Description", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "A comprehensive guide to C# programming.", 29.989999999999998, "C# Programming" },
                    { 2, "Learn how to build web applications using ASP.NET Core.", 35.5, "ASP.NET Core Development" },
                    { 3, "Master the Entity Framework Core ORM for .NET development.", 40.0, "Entity Framework Core In Action" },
                    { 4, "Everything you need to know about building Blazor WebAssembly applications.", 45.990000000000002, "Blazor WebAssembly: The Complete Guide" },
                    { 5, "Implement common design patterns in C# to improve code structure.", 50.0, "Design Patterns in C#" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}

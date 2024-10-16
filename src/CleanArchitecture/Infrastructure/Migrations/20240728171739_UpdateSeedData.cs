using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CleanArchitecture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a3314be5-4c77-4fb6-82ad-302014682a73"), new Guid("69db714f-9576-45ba-b5b7-f00649be02de") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a3314be5-4c77-4fb6-82ad-302014682a73"), new Guid("69db714f-9576-45ba-b5b7-f00649be03de") });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a3314be5-4c77-4fb6-82ad-302014682a73"), new Guid("69db714f-9576-45ba-b5b7-f00649be04de") });

            migrationBuilder.DeleteData(
                table: "ApplicationUser",
                keyColumn: "Id",
                keyValue: new Guid("69db714f-9576-45ba-b5b7-f00649be02de"));

            migrationBuilder.DeleteData(
                table: "ApplicationUser",
                keyColumn: "Id",
                keyValue: new Guid("69db714f-9576-45ba-b5b7-f00649be03de"));

            migrationBuilder.DeleteData(
                table: "ApplicationUser",
                keyColumn: "Id",
                keyValue: new Guid("69db714f-9576-45ba-b5b7-f00649be04de"));

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "SortOrder",
                table: "Media");

            migrationBuilder.UpdateData(
                table: "ApplicationUser",
                keyColumn: "Id",
                keyValue: new Guid("69db714f-9576-45ba-b5b7-f00649be01de"),
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "dd829aed-ba0a-48a8-bfa2-31502d8c4d70", "admin@gmail.com", "ADMIN@GMAIL.COM", "admin", "AQAAAAIAAYagAAAAEPBze+Pd4XI+Pv1FnmwBZHqp9ZHk+8tJzFuCEfZ/hKyNl5SLozdRTkiLpRpwWSQF0w==", "admin" });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b4314be5-4c77-4fb6-82ad-302014682b13"),
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "User", "User" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Media",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SortOrder",
                table: "Media",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ApplicationUser",
                keyColumn: "Id",
                keyValue: new Guid("69db714f-9576-45ba-b5b7-f00649be01de"),
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "a6456a8d-a4b0-4046-830b-a9731ebd5a6b", "admin1@gmail.com", "ADMIN1@GMAIL.COM", "admin1", "AQAAAAIAAYagAAAAEO9oNwPJdecRbKgHQ8tJRhEO15iZe0gUmQ+VNQBvooV1Q1mwLvHL00QzD6yS0BSo7g==", "admin1" });

            migrationBuilder.InsertData(
                table: "ApplicationUser",
                columns: new[] { "Id", "AccessFailedCount", "AvatarId", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("69db714f-9576-45ba-b5b7-f00649be02de"), 0, null, "d905b15f-0823-4e9d-852f-828a2fc37a1a", "admin2@gmail.com", true, false, null, "Admin 2", "ADMIN2@GMAIL.COM", "Admin2", "AQAAAAIAAYagAAAAEG1Yzxzo7CIUqt7WtmdliA4t61NXibkTCe5yOcT1MeuOsUj2qFT9dt2b8sjRShH11w==", null, false, "", false, "Admin2" },
                    { new Guid("69db714f-9576-45ba-b5b7-f00649be03de"), 0, null, "3ce89235-da10-4143-bb1b-533ac5b56e9d", "admin3@gmail.com", true, false, null, "Admin 3", "ADMIN3@GMAIL.COM", "Admin3", "AQAAAAIAAYagAAAAEJIRTCUCZAjaSR5khibEucQhHlPpyLDzCRw+0Ip7lIBjXGNEcicCmGwZSbjU8paEnw==", null, false, "", false, "Admin3" },
                    { new Guid("69db714f-9576-45ba-b5b7-f00649be04de"), 0, null, "c6b8f21b-5cfe-41bc-86a9-48b6ff373527", "admin4@gmail.com", true, false, null, "Admin 4", "ADMIN4@GMAIL.COM", "Admin4", "AQAAAAIAAYagAAAAEBCAreAzwYB2gvdJ0G7gALi+fUFmDgf7Ls0OuyGhVPexREP7REBzCErLQsrwXERGHg==", null, false, "", false, "Admin4" }
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b4314be5-4c77-4fb6-82ad-302014682b13"),
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Subscriber", "Subscriber" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("a3314be5-4c77-4fb6-82ad-302014682a73"), new Guid("69db714f-9576-45ba-b5b7-f00649be02de") },
                    { new Guid("a3314be5-4c77-4fb6-82ad-302014682a73"), new Guid("69db714f-9576-45ba-b5b7-f00649be03de") },
                    { new Guid("a3314be5-4c77-4fb6-82ad-302014682a73"), new Guid("69db714f-9576-45ba-b5b7-f00649be04de") }
                });
        }
    }
}

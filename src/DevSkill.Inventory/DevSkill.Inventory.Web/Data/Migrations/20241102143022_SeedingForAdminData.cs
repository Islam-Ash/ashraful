using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class SeedingForAdminData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("7443ee90-13eb-4828-b148-66ebe01f2364"), new Guid("d99e2960-9a65-44d5-95a8-ed95349c29ca") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7443ee90-13eb-4828-b148-66ebe01f2364"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("d99e2960-9a65-44d5-95a8-ed95349c29ca"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("497f026c-6861-4fd5-a68d-461c379a880a"), null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("1c31d902-5bc0-4a87-9609-cc98b14d552c"), 0, "3ae75556-4b4d-4d31-a5ec-09c6d6e7c3c0", "aislam.cse.1023@gmail.com", true, "Ashraful", "Islam", false, null, "AISLAM.CSE.1023@GMAIL.COM", "AISLAM.CSE.1023@GMAIL.COM", "AQAAAAIAAYagAAAAEGsyZuPbFm3ARME9H4m/hZEcHAw9J4/Z56zhuqa/Gg7uHTu3HZpbJ0dlH3zfSglRRw==", "01518745450", false, null, false, "aislam.cse.1023@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("497f026c-6861-4fd5-a68d-461c379a880a"), new Guid("1c31d902-5bc0-4a87-9609-cc98b14d552c") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("497f026c-6861-4fd5-a68d-461c379a880a"), new Guid("1c31d902-5bc0-4a87-9609-cc98b14d552c") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("497f026c-6861-4fd5-a68d-461c379a880a"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("1c31d902-5bc0-4a87-9609-cc98b14d552c"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("7443ee90-13eb-4828-b148-66ebe01f2364"), null, "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("d99e2960-9a65-44d5-95a8-ed95349c29ca"), 0, "217ee1a7-b291-4dcc-8919-daa1e755f2b8", "aislam.cse.1023@gmail.com", true, "Ashraful", "Islam", false, null, "AISLAM.CSE.1023@GMAIL.COM", "AISLAM.CSE.1023@GMAIL.COM", "AQAAAAIAAYagAAAAEKWZT5FDEkZFApi3LLkrNhYrIEz/iIUHhxujQVOWqcAm7RY0QmL1u+Oy7dcetLnWwA==", "01518745450", false, null, false, "aislam.cse.1023@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("7443ee90-13eb-4828-b148-66ebe01f2364"), new Guid("d99e2960-9a65-44d5-95a8-ed95349c29ca") });
        }
    }
}

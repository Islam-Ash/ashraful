using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeededAdminUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { new Guid("8f62e3db-326f-4838-936a-9ca4e3bc8747"), "d88d8668-34c1-4702-ba44-df9f4f377705", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("fdac9f9b-71a4-4dae-9091-bb88361a9976"), 0, "66a15696-a0e8-4462-8503-b94fe50549fc", "aislam.cse.1023@gmail.com", true, "Ashraful", "Islam", false, null, "AISLAM.CSE.1023@GMAIL.COM", "AISLAM.CSE.1023@GMAIL.COM", "AQAAAAIAAYagAAAAEKGR+MPA2B4jA1uXoT1L48zP5X2WggA71rM98t/nHRXD+RM2mcbdARTOc/DJsoA4qQ==", "01518745450", false, "3d0f8794-7fb5-4bed-8f68-75854fdb0440", false, "aislam.cse.1023@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("8f62e3db-326f-4838-936a-9ca4e3bc8747"), new Guid("fdac9f9b-71a4-4dae-9091-bb88361a9976") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("8f62e3db-326f-4838-936a-9ca4e3bc8747"), new Guid("fdac9f9b-71a4-4dae-9091-bb88361a9976") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8f62e3db-326f-4838-936a-9ca4e3bc8747"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("fdac9f9b-71a4-4dae-9091-bb88361a9976"));

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
    }
}

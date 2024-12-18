using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleApplication.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "UserName");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Password", "Role", "UserName" },
                values: new object[] { 10, "ashraful", 0, "Ashraful Islam" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 10);

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Username");
        }
    }
}

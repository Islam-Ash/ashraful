using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConsoleApplication.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataInTermTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Terms",
                columns: new[] { "TermId", "TermName" },
                values: new object[,]
                {
                    { 1, "First" },
                    { 2, "Mid" },
                    { 3, "Final" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Terms",
                keyColumn: "TermId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Terms",
                keyColumn: "TermId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Terms",
                keyColumn: "TermId",
                keyValue: 3);
        }
    }
}

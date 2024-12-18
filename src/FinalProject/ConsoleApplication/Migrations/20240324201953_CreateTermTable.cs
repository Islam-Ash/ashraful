using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleApplication.Migrations
{
    /// <inheritdoc />
    public partial class CreateTermTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "Terms",
                columns: table => new
                {
                    TermId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terms", x => x.TermId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Terms");

           
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleApplication.Migrations
{
    /// <inheritdoc />
    public partial class CreateExamGradesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "ExamGrades",
                columns: table => new
                {
                    ExamGradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    TermId = table.Column<int>(type: "int", nullable: false),
                    ExamGradeValue = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamGrades", x => x.ExamGradeId);
                    table.ForeignKey(
                        name: "FK_ExamGrades_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamGrades_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamGrades_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "TermId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamGrades_StudentId",
                table: "ExamGrades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamGrades_SubjectId",
                table: "ExamGrades",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamGrades_TermId",
                table: "ExamGrades",
                column: "TermId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamGrades");
        }
    }
}

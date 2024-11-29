using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationalSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addnewtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LessonDescription",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SubLessonID",
                table: "Lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FileSubmissions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileSubmissions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FileSubmissions_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FileSubmissions_Assessments_AssessmentID",
                        column: x => x.AssessmentID,
                        principalTable: "Assessments",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Rubrics",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentID = table.Column<int>(type: "int", nullable: false),
                    Criterion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxPoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rubrics", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rubrics_Assessments_AssessmentID",
                        column: x => x.AssessmentID,
                        principalTable: "Assessments",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SubLessons",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubLessons", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SubLessons_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TextSubmissions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextSubmissions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TextSubmissions_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TextSubmissions_Assessments_AssessmentID",
                        column: x => x.AssessmentID,
                        principalTable: "Assessments",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_SubLessonID",
                table: "Lessons",
                column: "SubLessonID");

            migrationBuilder.CreateIndex(
                name: "IX_FileSubmissions_AssessmentID",
                table: "FileSubmissions",
                column: "AssessmentID");

            migrationBuilder.CreateIndex(
                name: "IX_FileSubmissions_UserID",
                table: "FileSubmissions",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Rubrics_AssessmentID",
                table: "Rubrics",
                column: "AssessmentID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubLessons_CourseID",
                table: "SubLessons",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_TextSubmissions_AssessmentID",
                table: "TextSubmissions",
                column: "AssessmentID");

            migrationBuilder.CreateIndex(
                name: "IX_TextSubmissions_UserID",
                table: "TextSubmissions",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_SubLessons_SubLessonID",
                table: "Lessons",
                column: "SubLessonID",
                principalTable: "SubLessons",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_SubLessons_SubLessonID",
                table: "Lessons");

            migrationBuilder.DropTable(
                name: "FileSubmissions");

            migrationBuilder.DropTable(
                name: "Rubrics");

            migrationBuilder.DropTable(
                name: "SubLessons");

            migrationBuilder.DropTable(
                name: "TextSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_SubLessonID",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "LessonDescription",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "SubLessonID",
                table: "Lessons");
        }
    }
}

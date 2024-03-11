using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddProjectExcercise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectExercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ThemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectExercise_Theme_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Theme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourseProjectExercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkToProject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentCourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourseProjectExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentCourseProjectExercise_ProjectExercise_ProjectExerciseId",
                        column: x => x.ProjectExerciseId,
                        principalTable: "ProjectExercise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCourseProjectExercise_StudentCourse_StudentCourseId",
                        column: x => x.StudentCourseId,
                        principalTable: "StudentCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectExercise_ThemeId",
                table: "ProjectExercise",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseProjectExercise_ProjectExerciseId",
                table: "StudentCourseProjectExercise",
                column: "ProjectExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourseProjectExercise_StudentCourseId",
                table: "StudentCourseProjectExercise",
                column: "StudentCourseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentCourseProjectExercise");

            migrationBuilder.DropTable(
                name: "ProjectExercise");
        }
    }
}

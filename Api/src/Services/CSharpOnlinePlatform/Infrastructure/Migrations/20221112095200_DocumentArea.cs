using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class DocumentArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Documentation_Name",
                table: "Documentation");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Documentation");

            migrationBuilder.AddColumn<int>(
                name: "Area",
                table: "Documentation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "Documentation");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Documentation",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documentation_Name",
                table: "Documentation",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }
    }
}

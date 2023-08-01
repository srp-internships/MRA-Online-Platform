using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ChangeTestDesign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectVariant",
                table: "Test");

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "VariantTest",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "VariantTest");

            migrationBuilder.AddColumn<Guid>(
                name: "CorrectVariant",
                table: "Test",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}

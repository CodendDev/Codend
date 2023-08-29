using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectDescription",
                table: "Project",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "Project",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectDescription",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "Project");
        }
    }
}

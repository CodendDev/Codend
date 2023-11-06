using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class SprintProjectTaskPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "SprintProjectTask",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "SprintProjectTask");
        }
    }
}

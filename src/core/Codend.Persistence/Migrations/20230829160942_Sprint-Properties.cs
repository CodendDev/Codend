using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SprintProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Sprint",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Sprint",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Goal",
                table: "Sprint",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Sprint",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "Sprint");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Sprint");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "Sprint");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Sprint");
        }
    }
}

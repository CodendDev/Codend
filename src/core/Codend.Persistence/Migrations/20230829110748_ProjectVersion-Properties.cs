using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectVersionProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Changelog",
                table: "ProjectVersion",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "ProjectVersion",
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "VersionName",
                table: "ProjectVersion",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VersionTag",
                table: "ProjectVersion",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Changelog",
                table: "ProjectVersion");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "ProjectVersion");

            migrationBuilder.DropColumn(
                name: "VersionName",
                table: "ProjectVersion");

            migrationBuilder.DropColumn(
                name: "VersionTag",
                table: "ProjectVersion");
        }
    }
}

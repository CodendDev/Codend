using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class ProjectDefaultStatusEpicStatusStoryStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "Story",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultStatusId",
                table: "Project",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "Epic",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Story_StatusId",
                table: "Story",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_DefaultStatusId",
                table: "Project",
                column: "DefaultStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Epic_StatusId",
                table: "Epic",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Epic_ProjectTaskStatus_StatusId",
                table: "Epic",
                column: "StatusId",
                principalTable: "ProjectTaskStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_ProjectTaskStatus_DefaultStatusId",
                table: "Project",
                column: "DefaultStatusId",
                principalTable: "ProjectTaskStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Story_ProjectTaskStatus_StatusId",
                table: "Story",
                column: "StatusId",
                principalTable: "ProjectTaskStatus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Epic_ProjectTaskStatus_StatusId",
                table: "Epic");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_ProjectTaskStatus_DefaultStatusId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Story_ProjectTaskStatus_StatusId",
                table: "Story");

            migrationBuilder.DropIndex(
                name: "IX_Story_StatusId",
                table: "Story");

            migrationBuilder.DropIndex(
                name: "IX_Project_DefaultStatusId",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Epic_StatusId",
                table: "Epic");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Story");

            migrationBuilder.DropColumn(
                name: "DefaultStatusId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Epic");
        }
    }
}

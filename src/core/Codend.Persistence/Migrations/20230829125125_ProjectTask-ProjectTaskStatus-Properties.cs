using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTaskProjectTaskStatusProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssigneeId",
                table: "ProjectTask",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProjectTask",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "ProjectTask",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProjectTask",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "ProjectTask",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "ProjectTaskPriority",
                table: "ProjectTask",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "ProjectTask",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ProjectTaskStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTaskStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTaskStatus_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_StatusId",
                table: "ProjectTask",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskStatus_ProjectId",
                table: "ProjectTaskStatus",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_ProjectTaskStatus_StatusId",
                table: "ProjectTask",
                column: "StatusId",
                principalTable: "ProjectTaskStatus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_ProjectTaskStatus_StatusId",
                table: "ProjectTask");

            migrationBuilder.DropTable(
                name: "ProjectTaskStatus");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTask_StatusId",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "ProjectTaskPriority",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "ProjectTask");
        }
    }
}

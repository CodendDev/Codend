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
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Sprint_SprintId",
                table: "ProjectTask");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTask_SprintId",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "SprintId",
                table: "ProjectTask");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Sprint",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Sprint",
                type: "datetime2(0)",
                precision: 0,
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
                type: "datetime2(0)",
                precision: 0,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ProjectTaskSprint",
                columns: table => new
                {
                    AssignedToSprintsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SprintProjectTasksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTaskSprint", x => new { x.AssignedToSprintsId, x.SprintProjectTasksId });
                    table.ForeignKey(
                        name: "FK_ProjectTaskSprint_ProjectTask_SprintProjectTasksId",
                        column: x => x.SprintProjectTasksId,
                        principalTable: "ProjectTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTaskSprint_Sprint_AssignedToSprintsId",
                        column: x => x.AssignedToSprintsId,
                        principalTable: "Sprint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SprintProjectTask",
                columns: table => new
                {
                    ProjectTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SprintId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SprintProjectTask", x => new { x.ProjectTaskId, x.SprintId });
                    table.ForeignKey(
                        name: "FK_SprintProjectTask_ProjectTask_ProjectTaskId",
                        column: x => x.ProjectTaskId,
                        principalTable: "ProjectTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SprintProjectTask_Sprint_SprintId",
                        column: x => x.SprintId,
                        principalTable: "Sprint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskSprint_SprintProjectTasksId",
                table: "ProjectTaskSprint",
                column: "SprintProjectTasksId");

            migrationBuilder.CreateIndex(
                name: "IX_SprintProjectTask_SprintId",
                table: "SprintProjectTask",
                column: "SprintId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTaskSprint");

            migrationBuilder.DropTable(
                name: "SprintProjectTask");

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

            migrationBuilder.AddColumn<Guid>(
                name: "SprintId",
                table: "ProjectTask",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_SprintId",
                table: "ProjectTask",
                column: "SprintId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Sprint_SprintId",
                table: "ProjectTask",
                column: "SprintId",
                principalTable: "Sprint",
                principalColumn: "Id");
        }
    }
}

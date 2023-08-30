using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_ProjectTaskId",
                table: "SprintProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_Sprint_SprintId",
                table: "SprintProjectTask");

            migrationBuilder.DropTable(
                name: "ProjectTaskSprint");

            migrationBuilder.RenameColumn(
                name: "SprintId",
                table: "SprintProjectTask",
                newName: "SprintProjectTasksId");

            migrationBuilder.RenameColumn(
                name: "ProjectTaskId",
                table: "SprintProjectTask",
                newName: "AssignedToSprintsId");

            migrationBuilder.RenameIndex(
                name: "IX_SprintProjectTask_SprintId",
                table: "SprintProjectTask",
                newName: "IX_SprintProjectTask_SprintProjectTasksId");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_SprintProjectTasksId",
                table: "SprintProjectTask",
                column: "SprintProjectTasksId",
                principalTable: "ProjectTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_Sprint_AssignedToSprintsId",
                table: "SprintProjectTask",
                column: "AssignedToSprintsId",
                principalTable: "Sprint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_SprintProjectTasksId",
                table: "SprintProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_Sprint_AssignedToSprintsId",
                table: "SprintProjectTask");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.RenameColumn(
                name: "SprintProjectTasksId",
                table: "SprintProjectTask",
                newName: "SprintId");

            migrationBuilder.RenameColumn(
                name: "AssignedToSprintsId",
                table: "SprintProjectTask",
                newName: "ProjectTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_SprintProjectTask_SprintProjectTasksId",
                table: "SprintProjectTask",
                newName: "IX_SprintProjectTask_SprintId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskSprint_SprintProjectTasksId",
                table: "ProjectTaskSprint",
                column: "SprintProjectTasksId");

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_ProjectTaskId",
                table: "SprintProjectTask",
                column: "ProjectTaskId",
                principalTable: "ProjectTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_Sprint_SprintId",
                table: "SprintProjectTask",
                column: "SprintId",
                principalTable: "Sprint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

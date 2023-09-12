using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AbstractProjectTaskRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_ProjectTaskStatus_StatusId",
                table: "ProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Project_ProjectId",
                table: "ProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_ProjectTaskId",
                table: "SprintProjectTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectTask",
                table: "ProjectTask");

            migrationBuilder.RenameTable(
                name: "ProjectTask",
                newName: "AbstractProjectTask");

            migrationBuilder.RenameColumn(
                name: "ProjectTaskId",
                table: "SprintProjectTask",
                newName: "AbstractProjectTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTask_StatusId",
                table: "AbstractProjectTask",
                newName: "IX_AbstractProjectTask_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTask_ProjectId",
                table: "AbstractProjectTask",
                newName: "IX_AbstractProjectTask_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AbstractProjectTask",
                table: "AbstractProjectTask",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AbstractProjectTask_ProjectTaskStatus_StatusId",
                table: "AbstractProjectTask",
                column: "StatusId",
                principalTable: "ProjectTaskStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AbstractProjectTask_Project_ProjectId",
                table: "AbstractProjectTask",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_AbstractProjectTask_AbstractProjectTaskId",
                table: "SprintProjectTask",
                column: "AbstractProjectTaskId",
                principalTable: "AbstractProjectTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbstractProjectTask_ProjectTaskStatus_StatusId",
                table: "AbstractProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_AbstractProjectTask_Project_ProjectId",
                table: "AbstractProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_AbstractProjectTask_AbstractProjectTaskId",
                table: "SprintProjectTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AbstractProjectTask",
                table: "AbstractProjectTask");

            migrationBuilder.RenameTable(
                name: "AbstractProjectTask",
                newName: "ProjectTask");

            migrationBuilder.RenameColumn(
                name: "AbstractProjectTaskId",
                table: "SprintProjectTask",
                newName: "ProjectTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_AbstractProjectTask_StatusId",
                table: "ProjectTask",
                newName: "IX_ProjectTask_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_AbstractProjectTask_ProjectId",
                table: "ProjectTask",
                newName: "IX_ProjectTask_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectTask",
                table: "ProjectTask",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_ProjectTaskStatus_StatusId",
                table: "ProjectTask",
                column: "StatusId",
                principalTable: "ProjectTaskStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Project_ProjectId",
                table: "ProjectTask",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_ProjectTaskId",
                table: "SprintProjectTask",
                column: "ProjectTaskId",
                principalTable: "ProjectTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

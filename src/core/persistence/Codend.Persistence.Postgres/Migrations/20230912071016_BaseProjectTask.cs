using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class BaseProjectTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_ProjectTaskBaseId",
                table: "SprintProjectTask");

            migrationBuilder.RenameColumn(
                name: "ProjectTaskBaseId",
                table: "SprintProjectTask",
                newName: "BaseProjectTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_BaseProjectTaskId",
                table: "SprintProjectTask",
                column: "BaseProjectTaskId",
                principalTable: "ProjectTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_BaseProjectTaskId",
                table: "SprintProjectTask");

            migrationBuilder.RenameColumn(
                name: "BaseProjectTaskId",
                table: "SprintProjectTask",
                newName: "ProjectTaskBaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_ProjectTaskBaseId",
                table: "SprintProjectTask",
                column: "ProjectTaskBaseId",
                principalTable: "ProjectTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

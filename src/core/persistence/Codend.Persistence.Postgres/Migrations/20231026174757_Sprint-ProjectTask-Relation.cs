using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class SprintProjectTaskRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_BaseProjectTaskId",
                table: "SprintProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_Sprint_SprintId",
                table: "SprintProjectTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SprintProjectTask",
                table: "SprintProjectTask");

            migrationBuilder.RenameColumn(
                name: "BaseProjectTaskId",
                table: "SprintProjectTask",
                newName: "TaskId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "SprintProjectTask",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "SprintProjectTask",
                type: "timestamp(0) with time zone",
                precision: 0,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SprintProjectTask",
                table: "SprintProjectTask",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SprintProjectTask_TaskId",
                table: "SprintProjectTask",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_TaskId",
                table: "SprintProjectTask",
                column: "TaskId",
                principalTable: "ProjectTask",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_Sprint_SprintId",
                table: "SprintProjectTask",
                column: "SprintId",
                principalTable: "Sprint",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_TaskId",
                table: "SprintProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_Sprint_SprintId",
                table: "SprintProjectTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SprintProjectTask",
                table: "SprintProjectTask");

            migrationBuilder.DropIndex(
                name: "IX_SprintProjectTask_TaskId",
                table: "SprintProjectTask");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SprintProjectTask");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "SprintProjectTask");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "SprintProjectTask",
                newName: "BaseProjectTaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SprintProjectTask",
                table: "SprintProjectTask",
                columns: new[] { "BaseProjectTaskId", "SprintId" });

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_BaseProjectTaskId",
                table: "SprintProjectTask",
                column: "BaseProjectTaskId",
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

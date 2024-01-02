using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class SprintProjectTaskXD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "TaskId",
                table: "SprintProjectTask",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "EpicId",
                table: "SprintProjectTask",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StoryId",
                table: "SprintProjectTask",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SprintProjectTask_EpicId",
                table: "SprintProjectTask",
                column: "EpicId");

            migrationBuilder.CreateIndex(
                name: "IX_SprintProjectTask_StoryId",
                table: "SprintProjectTask",
                column: "StoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_Epic_EpicId",
                table: "SprintProjectTask",
                column: "EpicId",
                principalTable: "Epic",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_Story_StoryId",
                table: "SprintProjectTask",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_Epic_EpicId",
                table: "SprintProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_Story_StoryId",
                table: "SprintProjectTask");

            migrationBuilder.DropIndex(
                name: "IX_SprintProjectTask_EpicId",
                table: "SprintProjectTask");

            migrationBuilder.DropIndex(
                name: "IX_SprintProjectTask_StoryId",
                table: "SprintProjectTask");

            migrationBuilder.DropColumn(
                name: "EpicId",
                table: "SprintProjectTask");

            migrationBuilder.DropColumn(
                name: "StoryId",
                table: "SprintProjectTask");

            migrationBuilder.AlterColumn<Guid>(
                name: "TaskId",
                table: "SprintProjectTask",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}

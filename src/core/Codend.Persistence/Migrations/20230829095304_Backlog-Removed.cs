using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BacklogRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Backlog_BacklogId",
                table: "ProjectTask");

            migrationBuilder.DropTable(
                name: "Backlog");

            migrationBuilder.RenameColumn(
                name: "BacklogId",
                table: "ProjectTask",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTask_BacklogId",
                table: "ProjectTask",
                newName: "IX_ProjectTask_ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Project_ProjectId",
                table: "ProjectTask",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_Project_ProjectId",
                table: "ProjectTask");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "ProjectTask",
                newName: "BacklogId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectTask_ProjectId",
                table: "ProjectTask",
                newName: "IX_ProjectTask_BacklogId");

            migrationBuilder.CreateTable(
                name: "Backlog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backlog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Backlog_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Backlog_ProjectId",
                table: "Backlog",
                column: "ProjectId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_Backlog_BacklogId",
                table: "ProjectTask",
                column: "BacklogId",
                principalTable: "Backlog",
                principalColumn: "Id");
        }
    }
}

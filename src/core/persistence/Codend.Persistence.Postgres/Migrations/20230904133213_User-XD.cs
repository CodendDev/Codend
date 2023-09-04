using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class UserXD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_User_OwnerId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_User_AssigneeId",
                table: "ProjectTask");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_User_OwnerId",
                table: "ProjectTask");

            migrationBuilder.DropTable(
                name: "ProjectMember");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTask_AssigneeId",
                table: "ProjectTask");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTask_OwnerId",
                table: "ProjectTask");

            migrationBuilder.DropIndex(
                name: "IX_Project_OwnerId",
                table: "Project");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMember",
                columns: table => new
                {
                    ParticipatingInProjectsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMember", x => new { x.ParticipatingInProjectsId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProjectMember_Project_ParticipatingInProjectsId",
                        column: x => x.ParticipatingInProjectsId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMember_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_AssigneeId",
                table: "ProjectTask",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_OwnerId",
                table: "ProjectTask",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_OwnerId",
                table: "Project",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMember_UserId",
                table: "ProjectMember",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_User_OwnerId",
                table: "Project",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_User_AssigneeId",
                table: "ProjectTask",
                column: "AssigneeId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_User_OwnerId",
                table: "ProjectTask",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}

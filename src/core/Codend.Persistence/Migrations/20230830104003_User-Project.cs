using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Project",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ProjectMember",
                columns: table => new
                {
                    ParticipatingInProjectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectMembersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMember", x => new { x.ParticipatingInProjectsId, x.ProjectMembersId });
                    table.ForeignKey(
                        name: "FK_ProjectMember_Project_ParticipatingInProjectsId",
                        column: x => x.ParticipatingInProjectsId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMember_User_ProjectMembersId",
                        column: x => x.ProjectMembersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_OwnerId",
                table: "Project",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMember_ProjectMembersId",
                table: "ProjectMember",
                column: "ProjectMembersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_User_OwnerId",
                table: "Project",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_User_OwnerId",
                table: "Project");

            migrationBuilder.DropTable(
                name: "ProjectMember");

            migrationBuilder.DropIndex(
                name: "IX_Project_OwnerId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Project");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class StoryEpicRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EpicId",
                table: "Story",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Story_EpicId",
                table: "Story",
                column: "EpicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Story_Epic_EpicId",
                table: "Story",
                column: "EpicId",
                principalTable: "Epic",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Story_Epic_EpicId",
                table: "Story");

            migrationBuilder.DropIndex(
                name: "IX_Story_EpicId",
                table: "Story");

            migrationBuilder.DropColumn(
                name: "EpicId",
                table: "Story");
        }
    }
}

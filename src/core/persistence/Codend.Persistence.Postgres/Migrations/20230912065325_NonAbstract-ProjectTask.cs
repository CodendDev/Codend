using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class NonAbstractProjectTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_AbstractProjectTask_AbstractProjectTaskId",
                table: "SprintProjectTask");

            migrationBuilder.DropTable(
                name: "AbstractProjectTask");

            migrationBuilder.RenameColumn(
                name: "AbstractProjectTaskId",
                table: "SprintProjectTask",
                newName: "ProjectTaskBaseId");

            migrationBuilder.CreateTable(
                name: "ProjectTask",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ProjectTaskPriority = table.Column<int>(type: "integer", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssigneeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    EstimatedTime = table.Column<long>(type: "bigint", precision: 0, nullable: true),
                    StoryPoints = table.Column<long>(type: "bigint", nullable: true),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTask_ProjectTaskStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ProjectTaskStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectTask_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_ProjectId",
                table: "ProjectTask",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_StatusId",
                table: "ProjectTask",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_ProjectTaskBaseId",
                table: "SprintProjectTask",
                column: "ProjectTaskBaseId",
                principalTable: "ProjectTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SprintProjectTask_ProjectTask_ProjectTaskBaseId",
                table: "SprintProjectTask");

            migrationBuilder.DropTable(
                name: "ProjectTask");

            migrationBuilder.RenameColumn(
                name: "ProjectTaskBaseId",
                table: "SprintProjectTask",
                newName: "AbstractProjectTaskId");

            migrationBuilder.CreateTable(
                name: "AbstractProjectTask",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssigneeId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "timestamp(0) with time zone", precision: 0, nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EstimatedTime = table.Column<long>(type: "bigint", precision: 0, nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectTaskPriority = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoryPoints = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbstractProjectTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbstractProjectTask_ProjectTaskStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ProjectTaskStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AbstractProjectTask_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbstractProjectTask_ProjectId",
                table: "AbstractProjectTask",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AbstractProjectTask_StatusId",
                table: "AbstractProjectTask",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_SprintProjectTask_AbstractProjectTask_AbstractProjectTaskId",
                table: "SprintProjectTask",
                column: "AbstractProjectTaskId",
                principalTable: "AbstractProjectTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

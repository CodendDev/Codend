using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codend.Persistence.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class CodendScheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Epic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Epic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMember",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsFavourite = table.Column<bool>(type: "bit", nullable: false),
                    NotificationEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMember_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectTaskStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTaskStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTaskStatus_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectVersion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Changelog = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectVersion_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sprint",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Goal = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sprint_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Story",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    EpicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Story", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Story_Epic_EpicId",
                        column: x => x.EpicId,
                        principalTable: "Epic",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Story_ProjectTaskStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ProjectTaskStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Story_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectTask",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeletedOnUtc = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ProjectTaskPriority = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstimatedTime = table.Column<long>(type: "bigint", precision: 0, nullable: true),
                    StoryPoints = table.Column<long>(type: "bigint", nullable: true),
                    StoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false)
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
                    table.ForeignKey(
                        name: "FK_ProjectTask_Story_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Story",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SprintProjectTask",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SprintId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EpicId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SprintProjectTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SprintProjectTask_Epic_EpicId",
                        column: x => x.EpicId,
                        principalTable: "Epic",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SprintProjectTask_ProjectTask_TaskId",
                        column: x => x.TaskId,
                        principalTable: "ProjectTask",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SprintProjectTask_Sprint_SprintId",
                        column: x => x.SprintId,
                        principalTable: "Sprint",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SprintProjectTask_Story_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Story",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Epic_ProjectId",
                table: "Epic",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Epic_StatusId",
                table: "Epic",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_DefaultStatusId",
                table: "Project",
                column: "DefaultStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMember_ProjectId",
                table: "ProjectMember",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_ProjectId",
                table: "ProjectTask",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_StatusId",
                table: "ProjectTask",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_StoryId",
                table: "ProjectTask",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTaskStatus_ProjectId",
                table: "ProjectTaskStatus",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectVersion_ProjectId",
                table: "ProjectVersion",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Sprint_ProjectId",
                table: "Sprint",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SprintProjectTask_EpicId",
                table: "SprintProjectTask",
                column: "EpicId");

            migrationBuilder.CreateIndex(
                name: "IX_SprintProjectTask_SprintId",
                table: "SprintProjectTask",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_SprintProjectTask_StoryId",
                table: "SprintProjectTask",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SprintProjectTask_TaskId",
                table: "SprintProjectTask",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Story_EpicId",
                table: "Story",
                column: "EpicId");

            migrationBuilder.CreateIndex(
                name: "IX_Story_ProjectId",
                table: "Story",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Story_StatusId",
                table: "Story",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Epic_ProjectTaskStatus_StatusId",
                table: "Epic",
                column: "StatusId",
                principalTable: "ProjectTaskStatus",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Epic_Project_ProjectId",
                table: "Epic",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_ProjectTaskStatus_DefaultStatusId",
                table: "Project",
                column: "DefaultStatusId",
                principalTable: "ProjectTaskStatus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_ProjectTaskStatus_DefaultStatusId",
                table: "Project");

            migrationBuilder.DropTable(
                name: "ProjectMember");

            migrationBuilder.DropTable(
                name: "ProjectVersion");

            migrationBuilder.DropTable(
                name: "SprintProjectTask");

            migrationBuilder.DropTable(
                name: "ProjectTask");

            migrationBuilder.DropTable(
                name: "Sprint");

            migrationBuilder.DropTable(
                name: "Story");

            migrationBuilder.DropTable(
                name: "Epic");

            migrationBuilder.DropTable(
                name: "ProjectTaskStatus");

            migrationBuilder.DropTable(
                name: "Project");
        }
    }
}

﻿// <auto-generated />
using System;
using Codend.Persistence.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Codend.Persistence.Postgres.Migrations
{
    [DbContext(typeof(PostgresCodendDbContext))]
    partial class PostgresCodendDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Codend.Domain.Entities.BaseProjectTask", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AssigneeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasPrecision(0)
                        .HasColumnType("timestamp(0) with time zone");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("DeletedOnUtc")
                        .HasPrecision(0)
                        .HasColumnType("timestamp(0) with time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("DueDate");

                    b.Property<long?>("EstimatedTime")
                        .HasPrecision(0)
                        .HasColumnType("bigint");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<int>("Priority")
                        .HasColumnType("integer")
                        .HasColumnName("ProjectTaskPriority");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StatusId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("StoryId")
                        .HasColumnType("uuid");

                    b.Property<long?>("StoryPoints")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("StatusId");

                    b.HasIndex("StoryId");

                    b.ToTable("ProjectTask", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("BaseProjectTask");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Codend.Domain.Entities.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasPrecision(0)
                        .HasColumnType("timestamp(0) with time zone");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("DeletedOnUtc")
                        .HasPrecision(0)
                        .HasColumnType("timestamp(0) with time zone");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("Codend.Domain.Entities.ProjectMember", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasPrecision(0)
                        .HasColumnType("timestamp(0) with time zone");

                    b.Property<bool>("IsFavourite")
                        .HasColumnType("boolean");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectMember");
                });

            modelBuilder.Entity("Codend.Domain.Entities.ProjectTaskStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectTaskStatus");
                });

            modelBuilder.Entity("Codend.Domain.Entities.ProjectVersion", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasPrecision(0)
                        .HasColumnType("timestamp(0) with time zone");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("DeletedOnUtc")
                        .HasPrecision(0)
                        .HasColumnType("timestamp(0) with time zone");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ReleaseDate")
                        .HasPrecision(0)
                        .HasColumnType("timestamp(0) with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectVersion");
                });

            modelBuilder.Entity("Codend.Domain.Entities.Sprint", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasPrecision(0)
                        .HasColumnType("timestamp(0) with time zone");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("DeletedOnUtc")
                        .HasPrecision(0)
                        .HasColumnType("timestamp(0) with time zone");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Sprint");
                });

            modelBuilder.Entity("Codend.Domain.Entities.Story", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasPrecision(0)
                        .HasColumnType("timestamp(0) with time zone");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("DeletedOnUtc")
                        .HasPrecision(0)
                        .HasColumnType("timestamp(0) with time zone");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Story");
                });

            modelBuilder.Entity("SprintProjectTask", b =>
                {
                    b.Property<Guid>("BaseProjectTaskId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SprintId")
                        .HasColumnType("uuid");

                    b.HasKey("BaseProjectTaskId", "SprintId");

                    b.HasIndex("SprintId");

                    b.ToTable("SprintProjectTask");
                });

            modelBuilder.Entity("Codend.Domain.Entities.ProjectTask.Bugfix.BugfixProjectTask", b =>
                {
                    b.HasBaseType("Codend.Domain.Entities.BaseProjectTask");

                    b.HasDiscriminator().HasValue("BugfixProjectTask");
                });

            modelBuilder.Entity("Codend.Domain.Entities.BaseProjectTask", b =>
                {
                    b.HasOne("Codend.Domain.Entities.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Codend.Domain.Entities.ProjectTaskStatus", null)
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Codend.Domain.Entities.Story", null)
                        .WithMany()
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.OwnsOne("Codend.Domain.ValueObjects.ProjectTaskDescription", "Description", b1 =>
                        {
                            b1.Property<Guid>("BaseProjectTaskId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)")
                                .HasColumnName("Description");

                            b1.HasKey("BaseProjectTaskId");

                            b1.ToTable("ProjectTask");

                            b1.WithOwner()
                                .HasForeignKey("BaseProjectTaskId");
                        });

                    b.OwnsOne("Codend.Domain.ValueObjects.ProjectTaskName", "Name", b1 =>
                        {
                            b1.Property<Guid>("BaseProjectTaskId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("character varying(150)")
                                .HasColumnName("Name");

                            b1.HasKey("BaseProjectTaskId");

                            b1.ToTable("ProjectTask");

                            b1.WithOwner()
                                .HasForeignKey("BaseProjectTaskId");
                        });

                    b.Navigation("Description")
                        .IsRequired();

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("Codend.Domain.Entities.Project", b =>
                {
                    b.OwnsOne("Codend.Domain.ValueObjects.ProjectDescription", "Description", b1 =>
                        {
                            b1.Property<Guid>("ProjectId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .HasMaxLength(2000)
                                .HasColumnType("character varying(2000)")
                                .HasColumnName("Description");

                            b1.HasKey("ProjectId");

                            b1.ToTable("Project");

                            b1.WithOwner()
                                .HasForeignKey("ProjectId");
                        });

                    b.OwnsOne("Codend.Domain.ValueObjects.ProjectName", "Name", b1 =>
                        {
                            b1.Property<Guid>("ProjectId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("Name");

                            b1.HasKey("ProjectId");

                            b1.ToTable("Project");

                            b1.WithOwner()
                                .HasForeignKey("ProjectId");
                        });

                    b.Navigation("Description")
                        .IsRequired();

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("Codend.Domain.Entities.ProjectMember", b =>
                {
                    b.HasOne("Codend.Domain.Entities.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Codend.Domain.Entities.ProjectTaskStatus", b =>
                {
                    b.HasOne("Codend.Domain.Entities.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.OwnsOne("Codend.Domain.ValueObjects.ProjectTaskStatusName", "Name", b1 =>
                        {
                            b1.Property<Guid>("ProjectTaskStatusId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("character varying(150)")
                                .HasColumnName("Name");

                            b1.HasKey("ProjectTaskStatusId");

                            b1.ToTable("ProjectTaskStatus");

                            b1.WithOwner()
                                .HasForeignKey("ProjectTaskStatusId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("Codend.Domain.Entities.ProjectVersion", b =>
                {
                    b.HasOne("Codend.Domain.Entities.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.OwnsOne("Codend.Domain.ValueObjects.ProjectVersionChangelog", "Changelog", b1 =>
                        {
                            b1.Property<Guid>("ProjectVersionId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .HasMaxLength(3000)
                                .HasColumnType("character varying(3000)")
                                .HasColumnName("Changelog");

                            b1.HasKey("ProjectVersionId");

                            b1.ToTable("ProjectVersion");

                            b1.WithOwner()
                                .HasForeignKey("ProjectVersionId");
                        });

                    b.OwnsOne("Codend.Domain.ValueObjects.ProjectVersionName", "Name", b1 =>
                        {
                            b1.Property<Guid>("ProjectVersionId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("Name");

                            b1.HasKey("ProjectVersionId");

                            b1.ToTable("ProjectVersion");

                            b1.WithOwner()
                                .HasForeignKey("ProjectVersionId");
                        });

                    b.OwnsOne("Codend.Domain.ValueObjects.ProjectVersionTag", "Tag", b1 =>
                        {
                            b1.Property<Guid>("ProjectVersionId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)")
                                .HasColumnName("Tag");

                            b1.HasKey("ProjectVersionId");

                            b1.ToTable("ProjectVersion");

                            b1.WithOwner()
                                .HasForeignKey("ProjectVersionId");
                        });

                    b.Navigation("Changelog")
                        .IsRequired();

                    b.Navigation("Name")
                        .IsRequired();

                    b.Navigation("Tag")
                        .IsRequired();
                });

            modelBuilder.Entity("Codend.Domain.Entities.Sprint", b =>
                {
                    b.HasOne("Codend.Domain.Entities.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.OwnsOne("Codend.Domain.ValueObjects.SprintGoal", "Goal", b1 =>
                        {
                            b1.Property<Guid>("SprintId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .HasMaxLength(200)
                                .HasColumnType("character varying(200)")
                                .HasColumnName("Goal");

                            b1.HasKey("SprintId");

                            b1.ToTable("Sprint");

                            b1.WithOwner()
                                .HasForeignKey("SprintId");
                        });

                    b.OwnsOne("Codend.Domain.ValueObjects.SprintPeriod", "Period", b1 =>
                        {
                            b1.Property<Guid>("SprintId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("EndDate")
                                .HasPrecision(0)
                                .HasColumnType("timestamp(0) with time zone")
                                .HasColumnName("EndDate");

                            b1.Property<DateTime>("StartDate")
                                .HasPrecision(0)
                                .HasColumnType("timestamp(0) with time zone")
                                .HasColumnName("StartDate");

                            b1.HasKey("SprintId");

                            b1.ToTable("Sprint");

                            b1.WithOwner()
                                .HasForeignKey("SprintId");
                        });

                    b.Navigation("Goal")
                        .IsRequired();

                    b.Navigation("Period")
                        .IsRequired();
                });

            modelBuilder.Entity("Codend.Domain.Entities.Story", b =>
                {
                    b.HasOne("Codend.Domain.Entities.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.OwnsOne("Codend.Domain.ValueObjects.StoryDescription", "Description", b1 =>
                        {
                            b1.Property<Guid>("StoryId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(3000)
                                .HasColumnType("character varying(3000)")
                                .HasColumnName("Description");

                            b1.HasKey("StoryId");

                            b1.ToTable("Story");

                            b1.WithOwner()
                                .HasForeignKey("StoryId");
                        });

                    b.OwnsOne("Codend.Domain.ValueObjects.StoryName", "Name", b1 =>
                        {
                            b1.Property<Guid>("StoryId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("Name");

                            b1.HasKey("StoryId");

                            b1.ToTable("Story");

                            b1.WithOwner()
                                .HasForeignKey("StoryId");
                        });

                    b.Navigation("Description")
                        .IsRequired();

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("SprintProjectTask", b =>
                {
                    b.HasOne("Codend.Domain.Entities.BaseProjectTask", null)
                        .WithMany()
                        .HasForeignKey("BaseProjectTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Codend.Domain.Entities.Sprint", null)
                        .WithMany()
                        .HasForeignKey("SprintId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

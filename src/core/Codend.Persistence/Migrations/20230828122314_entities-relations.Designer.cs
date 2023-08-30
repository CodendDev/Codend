﻿// <auto-generated />
using System;
using Codend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Codend.Persistence.Migrations
{
    [DbContext(typeof(CodendApplicationDbContext))]
    [Migration("20230828122314_entities-relations")]
    partial class entitiesrelations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Codend.Domain.Core.Events.DomainEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ProjectTaskId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("ProjectTaskId");

                    b.ToTable("DomainEvent");
                });

            modelBuilder.Entity("Codend.Domain.Entities.Backlog", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId")
                        .IsUnique();

                    b.ToTable("Backlog");
                });

            modelBuilder.Entity("Codend.Domain.Entities.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.HasKey("Id");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("Codend.Domain.Entities.ProjectTask", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BacklogId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SprintId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BacklogId");

                    b.HasIndex("SprintId");

                    b.ToTable("ProjectTask");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ProjectTask");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Codend.Domain.Entities.ProjectVersion", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectVersion");
                });

            modelBuilder.Entity("Codend.Domain.Entities.Sprint", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Sprint");
                });

            modelBuilder.Entity("Codend.Domain.Entities.BugFixProjectTask", b =>
                {
                    b.HasBaseType("Codend.Domain.Entities.ProjectTask");

                    b.HasDiscriminator().HasValue("BugFixProjectTask");
                });

            modelBuilder.Entity("Codend.Domain.Core.Events.DomainEvent", b =>
                {
                    b.HasOne("Codend.Domain.Entities.Project", null)
                        .WithMany("DomainEvents")
                        .HasForeignKey("ProjectId");

                    b.HasOne("Codend.Domain.Entities.ProjectTask", null)
                        .WithMany("DomainEvents")
                        .HasForeignKey("ProjectTaskId");
                });

            modelBuilder.Entity("Codend.Domain.Entities.Backlog", b =>
                {
                    b.HasOne("Codend.Domain.Entities.Project", null)
                        .WithOne("Backlog")
                        .HasForeignKey("Codend.Domain.Entities.Backlog", "ProjectId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Codend.Domain.Entities.ProjectTask", b =>
                {
                    b.HasOne("Codend.Domain.Entities.Backlog", null)
                        .WithMany("ProjectTasks")
                        .HasForeignKey("BacklogId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Codend.Domain.Entities.Sprint", null)
                        .WithMany("SprintProjectTasks")
                        .HasForeignKey("SprintId");
                });

            modelBuilder.Entity("Codend.Domain.Entities.ProjectVersion", b =>
                {
                    b.HasOne("Codend.Domain.Entities.Project", null)
                        .WithMany("ProjectVersions")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Codend.Domain.Entities.Sprint", b =>
                {
                    b.HasOne("Codend.Domain.Entities.Project", null)
                        .WithMany("Sprints")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Codend.Domain.Entities.Backlog", b =>
                {
                    b.Navigation("ProjectTasks");
                });

            modelBuilder.Entity("Codend.Domain.Entities.Project", b =>
                {
                    b.Navigation("Backlog")
                        .IsRequired();

                    b.Navigation("DomainEvents");

                    b.Navigation("ProjectVersions");

                    b.Navigation("Sprints");
                });

            modelBuilder.Entity("Codend.Domain.Entities.ProjectTask", b =>
                {
                    b.Navigation("DomainEvents");
                });

            modelBuilder.Entity("Codend.Domain.Entities.Sprint", b =>
                {
                    b.Navigation("SprintProjectTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
﻿// <auto-generated />
using System;
using InteractiveScheduleUad.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InteractiveScheduleUad.Api.Migrations
{
    [DbContext(typeof(InteractiveScheduleUadApiDbContext))]
    [Migration("20231119030520_NewArchitecture")]
    partial class NewArchitecture
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)");

                    b.Property<DateTime>("Published")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Bio")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Abbreviation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Link")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.Lesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClassType")
                        .HasColumnType("integer");

                    b.Property<int?>("RoomId")
                        .HasColumnType("integer");

                    b.Property<int?>("ScheduleLessonId")
                        .HasColumnType("integer");

                    b.Property<int>("Sequence")
                        .HasColumnType("integer");

                    b.Property<int?>("SubjectId")
                        .HasColumnType("integer");

                    b.Property<int?>("TeacherId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.HasIndex("ScheduleLessonId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.RevokedToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Jti")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("RevokedTokens");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.ScheduleLesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("LessonId")
                        .HasColumnType("integer");

                    b.Property<int>("StudentsGroupId")
                        .HasColumnType("integer");

                    b.Property<int>("TimeContextId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ScheduleLessons");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.StudentsGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("FirstWeekScheduleId")
                        .HasColumnType("integer");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ScheduleLessonId")
                        .HasColumnType("integer");

                    b.Property<int?>("SecondWeekScheduleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FirstWeekScheduleId");

                    b.HasIndex("ScheduleLessonId");

                    b.HasIndex("SecondWeekScheduleId");

                    b.ToTable("StudentsGroups");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.Property<string>("Qualifications")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.TimeContext", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("LessonIndex")
                        .HasColumnType("integer");

                    b.Property<int>("WeekDay")
                        .HasColumnType("integer");

                    b.Property<int>("WeekIndex")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("TimeContexts");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<int>("UserRole")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.WeekSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("WeekSchedule");
                });

            modelBuilder.Entity("ScheduleLessonTimeContext", b =>
                {
                    b.Property<int>("ScheduleLessonsId")
                        .HasColumnType("integer");

                    b.Property<int>("TimeContextsId")
                        .HasColumnType("integer");

                    b.HasKey("ScheduleLessonsId", "TimeContextsId");

                    b.HasIndex("TimeContextsId");

                    b.ToTable("ScheduleLessonTimeContext");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.Article", b =>
                {
                    b.HasOne("InteractiveScheduleUad.Api.Models.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.Lesson", b =>
                {
                    b.HasOne("InteractiveScheduleUad.Api.Models.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId");

                    b.HasOne("InteractiveScheduleUad.Api.Models.ScheduleLesson", null)
                        .WithMany("Lessons")
                        .HasForeignKey("ScheduleLessonId");

                    b.HasOne("InteractiveScheduleUad.Api.Models.Subject", "Subject")
                        .WithMany()
                        .HasForeignKey("SubjectId");

                    b.HasOne("InteractiveScheduleUad.Api.Models.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherId");

                    b.Navigation("Room");

                    b.Navigation("Subject");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.StudentsGroup", b =>
                {
                    b.HasOne("InteractiveScheduleUad.Api.Models.WeekSchedule", "FirstWeekSchedule")
                        .WithMany()
                        .HasForeignKey("FirstWeekScheduleId");

                    b.HasOne("InteractiveScheduleUad.Api.Models.ScheduleLesson", null)
                        .WithMany("StudentsGroups")
                        .HasForeignKey("ScheduleLessonId");

                    b.HasOne("InteractiveScheduleUad.Api.Models.WeekSchedule", "SecondWeekSchedule")
                        .WithMany()
                        .HasForeignKey("SecondWeekScheduleId");

                    b.Navigation("FirstWeekSchedule");

                    b.Navigation("SecondWeekSchedule");
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.Teacher", b =>
                {
                    b.HasOne("InteractiveScheduleUad.Api.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("ScheduleLessonTimeContext", b =>
                {
                    b.HasOne("InteractiveScheduleUad.Api.Models.ScheduleLesson", null)
                        .WithMany()
                        .HasForeignKey("ScheduleLessonsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InteractiveScheduleUad.Api.Models.TimeContext", null)
                        .WithMany()
                        .HasForeignKey("TimeContextsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InteractiveScheduleUad.Api.Models.ScheduleLesson", b =>
                {
                    b.Navigation("Lessons");

                    b.Navigation("StudentsGroups");
                });
#pragma warning restore 612, 618
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InteractiveScheduleUad.Api.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Departments",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: false),
                Abbreviation = table.Column<string>(type: "text", nullable: false),
                Link = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Departments", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "RevokedTokens",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Jti = table.Column<Guid>(type: "uuid", nullable: false),
                ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RevokedTokens", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Rooms",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Rooms", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Subjects",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Subjects", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Username = table.Column<string>(type: "text", nullable: false),
                PasswordHash = table.Column<byte[]>(type: "bytea", nullable: false),
                PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: false),
                UserRole = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "WeekSchedule",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_WeekSchedule", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Teachers",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                FirstName = table.Column<string>(type: "text", nullable: false),
                LastName = table.Column<string>(type: "text", nullable: false),
                MiddleName = table.Column<string>(type: "text", nullable: true),
                Email = table.Column<string>(type: "text", nullable: true),
                Qualifications = table.Column<string>(type: "text", nullable: true),
                DepartmentId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Teachers", x => x.Id);
                table.ForeignKey(
                    name: "FK_Teachers_Departments_DepartmentId",
                    column: x => x.DepartmentId,
                    principalTable: "Departments",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "StudentsGroups",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                GroupName = table.Column<string>(type: "text", nullable: false),
                FirstWeekScheduleId = table.Column<int>(type: "integer", nullable: true),
                SecondWeekScheduleId = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_StudentsGroups", x => x.Id);
                table.ForeignKey(
                    name: "FK_StudentsGroups_WeekSchedule_FirstWeekScheduleId",
                    column: x => x.FirstWeekScheduleId,
                    principalTable: "WeekSchedule",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_StudentsGroups_WeekSchedule_SecondWeekScheduleId",
                    column: x => x.SecondWeekScheduleId,
                    principalTable: "WeekSchedule",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Lesson",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Sequence = table.Column<int>(type: "integer", nullable: false),
                SubjectId = table.Column<int>(type: "integer", nullable: true),
                TeacherId = table.Column<int>(type: "integer", nullable: true),
                RoomId = table.Column<int>(type: "integer", nullable: true),
                ClassType = table.Column<int>(type: "integer", nullable: true),
                WeekScheduleId = table.Column<int>(type: "integer", nullable: true),
                WeekScheduleId1 = table.Column<int>(type: "integer", nullable: true),
                WeekScheduleId2 = table.Column<int>(type: "integer", nullable: true),
                WeekScheduleId3 = table.Column<int>(type: "integer", nullable: true),
                WeekScheduleId4 = table.Column<int>(type: "integer", nullable: true),
                WeekScheduleId5 = table.Column<int>(type: "integer", nullable: true),
                WeekScheduleId6 = table.Column<int>(type: "integer", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Lesson", x => x.Id);
                table.ForeignKey(
                    name: "FK_Lesson_Rooms_RoomId",
                    column: x => x.RoomId,
                    principalTable: "Rooms",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Lesson_Subjects_SubjectId",
                    column: x => x.SubjectId,
                    principalTable: "Subjects",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Lesson_Teachers_TeacherId",
                    column: x => x.TeacherId,
                    principalTable: "Teachers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Lesson_WeekSchedule_WeekScheduleId",
                    column: x => x.WeekScheduleId,
                    principalTable: "WeekSchedule",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Lesson_WeekSchedule_WeekScheduleId1",
                    column: x => x.WeekScheduleId1,
                    principalTable: "WeekSchedule",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Lesson_WeekSchedule_WeekScheduleId2",
                    column: x => x.WeekScheduleId2,
                    principalTable: "WeekSchedule",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Lesson_WeekSchedule_WeekScheduleId3",
                    column: x => x.WeekScheduleId3,
                    principalTable: "WeekSchedule",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Lesson_WeekSchedule_WeekScheduleId4",
                    column: x => x.WeekScheduleId4,
                    principalTable: "WeekSchedule",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Lesson_WeekSchedule_WeekScheduleId5",
                    column: x => x.WeekScheduleId5,
                    principalTable: "WeekSchedule",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Lesson_WeekSchedule_WeekScheduleId6",
                    column: x => x.WeekScheduleId6,
                    principalTable: "WeekSchedule",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Lesson_RoomId",
            table: "Lesson",
            column: "RoomId");

        migrationBuilder.CreateIndex(
            name: "IX_Lesson_SubjectId",
            table: "Lesson",
            column: "SubjectId");

        migrationBuilder.CreateIndex(
            name: "IX_Lesson_TeacherId",
            table: "Lesson",
            column: "TeacherId");

        migrationBuilder.CreateIndex(
            name: "IX_Lesson_WeekScheduleId",
            table: "Lesson",
            column: "WeekScheduleId");

        migrationBuilder.CreateIndex(
            name: "IX_Lesson_WeekScheduleId1",
            table: "Lesson",
            column: "WeekScheduleId1");

        migrationBuilder.CreateIndex(
            name: "IX_Lesson_WeekScheduleId2",
            table: "Lesson",
            column: "WeekScheduleId2");

        migrationBuilder.CreateIndex(
            name: "IX_Lesson_WeekScheduleId3",
            table: "Lesson",
            column: "WeekScheduleId3");

        migrationBuilder.CreateIndex(
            name: "IX_Lesson_WeekScheduleId4",
            table: "Lesson",
            column: "WeekScheduleId4");

        migrationBuilder.CreateIndex(
            name: "IX_Lesson_WeekScheduleId5",
            table: "Lesson",
            column: "WeekScheduleId5");

        migrationBuilder.CreateIndex(
            name: "IX_Lesson_WeekScheduleId6",
            table: "Lesson",
            column: "WeekScheduleId6");

        migrationBuilder.CreateIndex(
            name: "IX_StudentsGroups_FirstWeekScheduleId",
            table: "StudentsGroups",
            column: "FirstWeekScheduleId");

        migrationBuilder.CreateIndex(
            name: "IX_StudentsGroups_SecondWeekScheduleId",
            table: "StudentsGroups",
            column: "SecondWeekScheduleId");

        migrationBuilder.CreateIndex(
            name: "IX_Teachers_DepartmentId",
            table: "Teachers",
            column: "DepartmentId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Lesson");

        migrationBuilder.DropTable(
            name: "RevokedTokens");

        migrationBuilder.DropTable(
            name: "StudentsGroups");

        migrationBuilder.DropTable(
            name: "Users");

        migrationBuilder.DropTable(
            name: "Rooms");

        migrationBuilder.DropTable(
            name: "Subjects");

        migrationBuilder.DropTable(
            name: "Teachers");

        migrationBuilder.DropTable(
            name: "WeekSchedule");

        migrationBuilder.DropTable(
            name: "Departments");
    }
}
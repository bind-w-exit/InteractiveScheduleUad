using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InteractiveScheduleUad.Api.Migrations
{
    /// <inheritdoc />
    public partial class NewArchitecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId1",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId2",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId3",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId4",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId5",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId6",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId7",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsGroups_Schedules_FirstWeekScheduleId",
                table: "StudentsGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsGroups_Schedules_SecondWeekScheduleId",
                table: "StudentsGroups");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_WeekScheduleId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_WeekScheduleId1",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_WeekScheduleId2",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_WeekScheduleId3",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_WeekScheduleId4",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_WeekScheduleId5",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_WeekScheduleId6",
                table: "Lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "WeekScheduleId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "WeekScheduleId1",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "WeekScheduleId2",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "WeekScheduleId3",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "WeekScheduleId4",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "WeekScheduleId5",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "WeekScheduleId6",
                table: "Lessons");

            migrationBuilder.RenameTable(
                name: "Schedules",
                newName: "WeekSchedule");

            migrationBuilder.RenameColumn(
                name: "WeekScheduleId7",
                table: "Lessons",
                newName: "ScheduleLessonId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_WeekScheduleId7",
                table: "Lessons",
                newName: "IX_Lessons_ScheduleLessonId");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleLessonId",
                table: "StudentsGroups",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeekSchedule",
                table: "WeekSchedule",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ScheduleLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentsGroupId = table.Column<int>(type: "integer", nullable: false),
                    LessonId = table.Column<int>(type: "integer", nullable: false),
                    TimeContextId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleLessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeContexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LessonIndex = table.Column<int>(type: "integer", nullable: false),
                    WeekDay = table.Column<int>(type: "integer", nullable: false),
                    WeekIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeContexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleLessonTimeContext",
                columns: table => new
                {
                    ScheduleLessonsId = table.Column<int>(type: "integer", nullable: false),
                    TimeContextsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleLessonTimeContext", x => new { x.ScheduleLessonsId, x.TimeContextsId });
                    table.ForeignKey(
                        name: "FK_ScheduleLessonTimeContext_ScheduleLessons_ScheduleLessonsId",
                        column: x => x.ScheduleLessonsId,
                        principalTable: "ScheduleLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleLessonTimeContext_TimeContexts_TimeContextsId",
                        column: x => x.TimeContextsId,
                        principalTable: "TimeContexts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentsGroups_ScheduleLessonId",
                table: "StudentsGroups",
                column: "ScheduleLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleLessonTimeContext_TimeContextsId",
                table: "ScheduleLessonTimeContext",
                column: "TimeContextsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_ScheduleLessons_ScheduleLessonId",
                table: "Lessons",
                column: "ScheduleLessonId",
                principalTable: "ScheduleLessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsGroups_ScheduleLessons_ScheduleLessonId",
                table: "StudentsGroups",
                column: "ScheduleLessonId",
                principalTable: "ScheduleLessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsGroups_WeekSchedule_FirstWeekScheduleId",
                table: "StudentsGroups",
                column: "FirstWeekScheduleId",
                principalTable: "WeekSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsGroups_WeekSchedule_SecondWeekScheduleId",
                table: "StudentsGroups",
                column: "SecondWeekScheduleId",
                principalTable: "WeekSchedule",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_ScheduleLessons_ScheduleLessonId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsGroups_ScheduleLessons_ScheduleLessonId",
                table: "StudentsGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsGroups_WeekSchedule_FirstWeekScheduleId",
                table: "StudentsGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsGroups_WeekSchedule_SecondWeekScheduleId",
                table: "StudentsGroups");

            migrationBuilder.DropTable(
                name: "ScheduleLessonTimeContext");

            migrationBuilder.DropTable(
                name: "ScheduleLessons");

            migrationBuilder.DropTable(
                name: "TimeContexts");

            migrationBuilder.DropIndex(
                name: "IX_StudentsGroups_ScheduleLessonId",
                table: "StudentsGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeekSchedule",
                table: "WeekSchedule");

            migrationBuilder.DropColumn(
                name: "ScheduleLessonId",
                table: "StudentsGroups");

            migrationBuilder.RenameTable(
                name: "WeekSchedule",
                newName: "Schedules");

            migrationBuilder.RenameColumn(
                name: "ScheduleLessonId",
                table: "Lessons",
                newName: "WeekScheduleId7");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_ScheduleLessonId",
                table: "Lessons",
                newName: "IX_Lessons_WeekScheduleId7");

            migrationBuilder.AddColumn<int>(
                name: "WeekScheduleId",
                table: "Lessons",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeekScheduleId1",
                table: "Lessons",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeekScheduleId2",
                table: "Lessons",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeekScheduleId3",
                table: "Lessons",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeekScheduleId4",
                table: "Lessons",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeekScheduleId5",
                table: "Lessons",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeekScheduleId6",
                table: "Lessons",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_WeekScheduleId",
                table: "Lessons",
                column: "WeekScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_WeekScheduleId1",
                table: "Lessons",
                column: "WeekScheduleId1");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_WeekScheduleId2",
                table: "Lessons",
                column: "WeekScheduleId2");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_WeekScheduleId3",
                table: "Lessons",
                column: "WeekScheduleId3");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_WeekScheduleId4",
                table: "Lessons",
                column: "WeekScheduleId4");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_WeekScheduleId5",
                table: "Lessons",
                column: "WeekScheduleId5");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_WeekScheduleId6",
                table: "Lessons",
                column: "WeekScheduleId6");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId",
                table: "Lessons",
                column: "WeekScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId1",
                table: "Lessons",
                column: "WeekScheduleId1",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId2",
                table: "Lessons",
                column: "WeekScheduleId2",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId3",
                table: "Lessons",
                column: "WeekScheduleId3",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId4",
                table: "Lessons",
                column: "WeekScheduleId4",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId5",
                table: "Lessons",
                column: "WeekScheduleId5",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId6",
                table: "Lessons",
                column: "WeekScheduleId6",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Schedules_WeekScheduleId7",
                table: "Lessons",
                column: "WeekScheduleId7",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsGroups_Schedules_FirstWeekScheduleId",
                table: "StudentsGroups",
                column: "FirstWeekScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsGroups_Schedules_SecondWeekScheduleId",
                table: "StudentsGroups",
                column: "SecondWeekScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");
        }
    }
}

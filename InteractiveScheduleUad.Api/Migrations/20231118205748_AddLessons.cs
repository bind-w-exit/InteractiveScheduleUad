using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InteractiveScheduleUad.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddLessons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_Rooms_RoomId",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_Subjects_SubjectId",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_Teachers_TeacherId",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_WeekSchedule_WeekScheduleId",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_WeekSchedule_WeekScheduleId1",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_WeekSchedule_WeekScheduleId2",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_WeekSchedule_WeekScheduleId3",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_WeekSchedule_WeekScheduleId4",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_WeekSchedule_WeekScheduleId5",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_WeekSchedule_WeekScheduleId6",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsGroups_WeekSchedule_FirstWeekScheduleId",
                table: "StudentsGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsGroups_WeekSchedule_SecondWeekScheduleId",
                table: "StudentsGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeekSchedule",
                table: "WeekSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lesson",
                table: "Lesson");

            migrationBuilder.RenameTable(
                name: "WeekSchedule",
                newName: "Schedules");

            migrationBuilder.RenameTable(
                name: "Lesson",
                newName: "Lessons");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_WeekScheduleId6",
                table: "Lessons",
                newName: "IX_Lessons_WeekScheduleId6");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_WeekScheduleId5",
                table: "Lessons",
                newName: "IX_Lessons_WeekScheduleId5");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_WeekScheduleId4",
                table: "Lessons",
                newName: "IX_Lessons_WeekScheduleId4");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_WeekScheduleId3",
                table: "Lessons",
                newName: "IX_Lessons_WeekScheduleId3");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_WeekScheduleId2",
                table: "Lessons",
                newName: "IX_Lessons_WeekScheduleId2");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_WeekScheduleId1",
                table: "Lessons",
                newName: "IX_Lessons_WeekScheduleId1");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_WeekScheduleId",
                table: "Lessons",
                newName: "IX_Lessons_WeekScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_TeacherId",
                table: "Lessons",
                newName: "IX_Lessons_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_SubjectId",
                table: "Lessons",
                newName: "IX_Lessons_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_RoomId",
                table: "Lessons",
                newName: "IX_Lessons_RoomId");

            migrationBuilder.AddColumn<int>(
                name: "WeekScheduleId7",
                table: "Lessons",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lessons",
                table: "Lessons",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_WeekScheduleId7",
                table: "Lessons",
                column: "WeekScheduleId7");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Rooms_RoomId",
                table: "Lessons",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

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
                name: "FK_Lessons_Subjects_SubjectId",
                table: "Lessons",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Teachers_TeacherId",
                table: "Lessons",
                column: "TeacherId",
                principalTable: "Teachers",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Rooms_RoomId",
                table: "Lessons");

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
                name: "FK_Lessons_Subjects_SubjectId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Teachers_TeacherId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsGroups_Schedules_FirstWeekScheduleId",
                table: "StudentsGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsGroups_Schedules_SecondWeekScheduleId",
                table: "StudentsGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedules",
                table: "Schedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lessons",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_WeekScheduleId7",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "WeekScheduleId7",
                table: "Lessons");

            migrationBuilder.RenameTable(
                name: "Schedules",
                newName: "WeekSchedule");

            migrationBuilder.RenameTable(
                name: "Lessons",
                newName: "Lesson");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_WeekScheduleId6",
                table: "Lesson",
                newName: "IX_Lesson_WeekScheduleId6");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_WeekScheduleId5",
                table: "Lesson",
                newName: "IX_Lesson_WeekScheduleId5");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_WeekScheduleId4",
                table: "Lesson",
                newName: "IX_Lesson_WeekScheduleId4");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_WeekScheduleId3",
                table: "Lesson",
                newName: "IX_Lesson_WeekScheduleId3");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_WeekScheduleId2",
                table: "Lesson",
                newName: "IX_Lesson_WeekScheduleId2");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_WeekScheduleId1",
                table: "Lesson",
                newName: "IX_Lesson_WeekScheduleId1");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_WeekScheduleId",
                table: "Lesson",
                newName: "IX_Lesson_WeekScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_TeacherId",
                table: "Lesson",
                newName: "IX_Lesson_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_SubjectId",
                table: "Lesson",
                newName: "IX_Lesson_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_RoomId",
                table: "Lesson",
                newName: "IX_Lesson_RoomId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeekSchedule",
                table: "WeekSchedule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lesson",
                table: "Lesson",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_Rooms_RoomId",
                table: "Lesson",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_Subjects_SubjectId",
                table: "Lesson",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_Teachers_TeacherId",
                table: "Lesson",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_WeekSchedule_WeekScheduleId",
                table: "Lesson",
                column: "WeekScheduleId",
                principalTable: "WeekSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_WeekSchedule_WeekScheduleId1",
                table: "Lesson",
                column: "WeekScheduleId1",
                principalTable: "WeekSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_WeekSchedule_WeekScheduleId2",
                table: "Lesson",
                column: "WeekScheduleId2",
                principalTable: "WeekSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_WeekSchedule_WeekScheduleId3",
                table: "Lesson",
                column: "WeekScheduleId3",
                principalTable: "WeekSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_WeekSchedule_WeekScheduleId4",
                table: "Lesson",
                column: "WeekScheduleId4",
                principalTable: "WeekSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_WeekSchedule_WeekScheduleId5",
                table: "Lesson",
                column: "WeekScheduleId5",
                principalTable: "WeekSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_WeekSchedule_WeekScheduleId6",
                table: "Lesson",
                column: "WeekScheduleId6",
                principalTable: "WeekSchedule",
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
    }
}

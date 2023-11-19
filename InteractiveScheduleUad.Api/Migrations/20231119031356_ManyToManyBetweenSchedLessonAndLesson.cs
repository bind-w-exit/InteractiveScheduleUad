using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InteractiveScheduleUad.Api.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyBetweenSchedLessonAndLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_ScheduleLessons_ScheduleLessonId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_ScheduleLessonId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "ScheduleLessonId",
                table: "Lessons");

            migrationBuilder.CreateTable(
                name: "LessonScheduleLesson",
                columns: table => new
                {
                    LessonsId = table.Column<int>(type: "integer", nullable: false),
                    ScheduleLessonsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonScheduleLesson", x => new { x.LessonsId, x.ScheduleLessonsId });
                    table.ForeignKey(
                        name: "FK_LessonScheduleLesson_Lessons_LessonsId",
                        column: x => x.LessonsId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonScheduleLesson_ScheduleLessons_ScheduleLessonsId",
                        column: x => x.ScheduleLessonsId,
                        principalTable: "ScheduleLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonScheduleLesson_ScheduleLessonsId",
                table: "LessonScheduleLesson",
                column: "ScheduleLessonsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonScheduleLesson");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleLessonId",
                table: "Lessons",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ScheduleLessonId",
                table: "Lessons",
                column: "ScheduleLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_ScheduleLessons_ScheduleLessonId",
                table: "Lessons",
                column: "ScheduleLessonId",
                principalTable: "ScheduleLessons",
                principalColumn: "Id");
        }
    }
}

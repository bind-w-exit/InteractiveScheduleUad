using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InteractiveScheduleUad.Api.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyBetweenStudentsGroupAndScheduleLessonss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentsGroups_ScheduleLessons_ScheduleLessonId",
                table: "StudentsGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudentsGroups_ScheduleLessonId",
                table: "StudentsGroups");

            migrationBuilder.DropColumn(
                name: "ScheduleLessonId",
                table: "StudentsGroups");

            migrationBuilder.CreateTable(
                name: "ScheduleLessonStudentsGroup",
                columns: table => new
                {
                    ScheduleLessonsId = table.Column<int>(type: "integer", nullable: false),
                    StudentsGroupsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleLessonStudentsGroup", x => new { x.ScheduleLessonsId, x.StudentsGroupsId });
                    table.ForeignKey(
                        name: "FK_ScheduleLessonStudentsGroup_ScheduleLessons_ScheduleLessons~",
                        column: x => x.ScheduleLessonsId,
                        principalTable: "ScheduleLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScheduleLessonStudentsGroup_StudentsGroups_StudentsGroupsId",
                        column: x => x.StudentsGroupsId,
                        principalTable: "StudentsGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleLessonStudentsGroup_StudentsGroupsId",
                table: "ScheduleLessonStudentsGroup",
                column: "StudentsGroupsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduleLessonStudentsGroup");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleLessonId",
                table: "StudentsGroups",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentsGroups_ScheduleLessonId",
                table: "StudentsGroups",
                column: "ScheduleLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsGroups_ScheduleLessons_ScheduleLessonId",
                table: "StudentsGroups",
                column: "ScheduleLessonId",
                principalTable: "ScheduleLessons",
                principalColumn: "Id");
        }
    }
}

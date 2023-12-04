using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InteractiveScheduleUad.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUniquenessConstraintToFullContextOfScheduleLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ScheduleLessonJunctions_FullContextId",
                table: "ScheduleLessonJunctions");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleLessonJunctions_FullContextId",
                table: "ScheduleLessonJunctions",
                column: "FullContextId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ScheduleLessonJunctions_FullContextId",
                table: "ScheduleLessonJunctions");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleLessonJunctions_FullContextId",
                table: "ScheduleLessonJunctions",
                column: "FullContextId");
        }
    }
}

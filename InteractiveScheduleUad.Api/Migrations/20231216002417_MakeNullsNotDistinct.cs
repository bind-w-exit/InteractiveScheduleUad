using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InteractiveScheduleUad.Api.Migrations
{
    /// <inheritdoc />
    public partial class MakeNullsNotDistinct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lessons_ClassType_SubjectId_TeacherId_RoomId",
                table: "Lessons");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ClassType_SubjectId_TeacherId_RoomId",
                table: "Lessons",
                columns: new[] { "ClassType", "SubjectId", "TeacherId", "RoomId" },
                unique: true)
                .Annotation("Npgsql:NullsDistinct", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lessons_ClassType_SubjectId_TeacherId_RoomId",
                table: "Lessons");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ClassType_SubjectId_TeacherId_RoomId",
                table: "Lessons",
                columns: new[] { "ClassType", "SubjectId", "TeacherId", "RoomId" },
                unique: true);
        }
    }
}

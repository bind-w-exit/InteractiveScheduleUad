using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InteractiveScheduleUad.Api.Migrations
{
    /// <inheritdoc />
    public partial class Upd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "Lessons");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "Lessons",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

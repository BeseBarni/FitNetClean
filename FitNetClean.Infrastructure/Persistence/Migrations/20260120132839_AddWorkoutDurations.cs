using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitNetClean.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutDurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MainWorkoutDurationMinutes",
                table: "Workout",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WarmupDurationMinutes",
                table: "Workout",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainWorkoutDurationMinutes",
                table: "Workout");

            migrationBuilder.DropColumn(
                name: "WarmupDurationMinutes",
                table: "Workout");
        }
    }
}

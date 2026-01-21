using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitNetClean.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFavouriteExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteWorkout",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    WorkoutId = table.Column<long>(type: "bigint", nullable: false),
                    FavoritedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteWorkout", x => new { x.UserId, x.WorkoutId });
                    table.ForeignKey(
                        name: "FK_FavoriteWorkout_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavoriteWorkout_Workout_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteWorkout_WorkoutId",
                table: "FavoriteWorkout",
                column: "WorkoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteWorkout");
        }
    }
}

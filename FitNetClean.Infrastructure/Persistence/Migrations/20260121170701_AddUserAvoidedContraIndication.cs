using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitNetClean.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAvoidedContraIndication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAvoidedContraIndication",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ContraIndicationId = table.Column<long>(type: "bigint", nullable: false),
                    MarkedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAvoidedContraIndication", x => new { x.UserId, x.ContraIndicationId });
                    table.ForeignKey(
                        name: "FK_UserAvoidedContraIndication_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserAvoidedContraIndication_ContraIndication_ContraIndicati~",
                        column: x => x.ContraIndicationId,
                        principalTable: "ContraIndication",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAvoidedContraIndication_ContraIndicationId",
                table: "UserAvoidedContraIndication",
                column: "ContraIndicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAvoidedContraIndication");
        }
    }
}

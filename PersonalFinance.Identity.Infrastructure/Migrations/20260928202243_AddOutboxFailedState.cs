using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinance.Identity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxFailedState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OutboxMessages_ProcessedAt_NextRetryAt",
                table: "OutboxMessages");

            migrationBuilder.AddColumn<DateTime>(
                name: "FailedAt",
                table: "OutboxMessages",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessages_ProcessedAt_FailedAt_NextRetryAt",
                table: "OutboxMessages",
                columns: new[] { "ProcessedAt", "FailedAt", "NextRetryAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OutboxMessages_ProcessedAt_FailedAt_NextRetryAt",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "FailedAt",
                table: "OutboxMessages");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessages_ProcessedAt_NextRetryAt",
                table: "OutboxMessages",
                columns: new[] { "ProcessedAt", "NextRetryAt" });
        }
    }
}

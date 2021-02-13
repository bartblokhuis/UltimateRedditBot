using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations.UltimateDiscordDb
{
    public partial class BannedSubredditsRemoveUserIdField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BannedSubreddits_Users_UserId",
                table: "BannedSubreddits");

            migrationBuilder.DropIndex(
                name: "IX_BannedSubreddits_UserId",
                table: "BannedSubreddits");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BannedSubreddits");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UserId",
                table: "BannedSubreddits",
                type: "decimal(20,0)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BannedSubreddits_UserId",
                table: "BannedSubreddits",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BannedSubreddits_Users_UserId",
                table: "BannedSubreddits",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

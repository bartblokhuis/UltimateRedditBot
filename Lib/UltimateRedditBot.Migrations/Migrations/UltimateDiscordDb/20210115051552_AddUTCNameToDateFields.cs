using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations.UltimateDiscordDb
{
    public partial class AddUTCNameToDateFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                "UpdatedAt",
                "Guild",
                "UpdatedAtUTC");

            migrationBuilder.RenameColumn(
                "CreatedAt",
                "Guild",
                "CreatedAtUTC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                "UpdatedAtUTC",
                "Guild",
                "UpdatedAt");

            migrationBuilder.RenameColumn(
                "CreatedAtUTC",
                "Guild",
                "CreatedAt");
        }
    }
}
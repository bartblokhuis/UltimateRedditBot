using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations
{
    public partial class AddUTCNameToDateFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                "UpdatedAt",
                "Subreddits",
                "UpdatedAtUTC");

            migrationBuilder.RenameColumn(
                "CreatedAt",
                "Subreddits",
                "CreatedAtUTC");

            migrationBuilder.RenameColumn(
                "UpdatedAt",
                "Posts",
                "UpdatedAtUTC");

            migrationBuilder.RenameColumn(
                "CreatedAt",
                "Posts",
                "CreatedAtUTC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                "UpdatedAtUTC",
                "Subreddits",
                "UpdatedAt");

            migrationBuilder.RenameColumn(
                "CreatedAtUTC",
                "Subreddits",
                "CreatedAt");

            migrationBuilder.RenameColumn(
                "UpdatedAtUTC",
                "Posts",
                "UpdatedAt");

            migrationBuilder.RenameColumn(
                "CreatedAtUTC",
                "Posts",
                "CreatedAt");
        }
    }
}
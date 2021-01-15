using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations
{
    public partial class AddUTCNameToDateFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Subreddits",
                newName: "UpdatedAtUTC");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Subreddits",
                newName: "CreatedAtUTC");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Posts",
                newName: "UpdatedAtUTC");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Posts",
                newName: "CreatedAtUTC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAtUTC",
                table: "Subreddits",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAtUTC",
                table: "Subreddits",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUTC",
                table: "Posts",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAtUTC",
                table: "Posts",
                newName: "CreatedAt");
        }
    }
}

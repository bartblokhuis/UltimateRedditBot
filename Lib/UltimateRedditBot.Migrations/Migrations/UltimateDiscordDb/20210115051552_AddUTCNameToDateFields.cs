using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations.UltimateDiscordDb
{
    public partial class AddUTCNameToDateFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Guild",
                newName: "UpdatedAtUTC");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Guild",
                newName: "CreatedAtUTC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAtUTC",
                table: "Guild",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAtUTC",
                table: "Guild",
                newName: "CreatedAt");
        }
    }
}

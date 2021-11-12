using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class playlists_add_guild_limits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxQueueItems",
                table: "GuildSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxPlaylists",
                table: "GuildSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxSubscriptions",
                table: "GuildSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxQueueItems",
                table: "GuildSettings");

            migrationBuilder.DropColumn(
                name: "MaxPlaylists",
                table: "GuildSettings");

            migrationBuilder.DropColumn(
                name: "MaxSubscriptions",
                table: "GuildSettings");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    public partial class playlists_add_guild_fk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GuildId",
                table: "Playlists",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_GuildId",
                table: "Playlists",
                column: "GuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Guilds_GuildId",
                table: "Playlists",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Guilds_GuildId",
                table: "Playlists");

            migrationBuilder.DropIndex(
                name: "IX_Playlists_GuildId",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "Playlists");
        }
    }
}

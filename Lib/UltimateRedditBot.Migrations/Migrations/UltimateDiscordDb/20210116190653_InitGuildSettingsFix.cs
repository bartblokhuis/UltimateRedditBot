using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations.UltimateDiscordDb
{
    public partial class InitGuildSettingsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAtUTC",
                table: "DiscordChannels");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUTC",
                table: "DiscordChannels");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUTC",
                table: "GuildSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUTC",
                table: "GuildSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAtUTC",
                table: "GuildSettings");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUTC",
                table: "GuildSettings");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUTC",
                table: "DiscordChannels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUTC",
                table: "DiscordChannels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

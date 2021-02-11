using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations.UltimateDiscordDb
{
    public partial class InitGuildSettingsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "CreatedAtUTC",
                "DiscordChannels");

            migrationBuilder.DropColumn(
                "UpdatedAtUTC",
                "DiscordChannels");

            migrationBuilder.AddColumn<DateTime>(
                "CreatedAtUTC",
                "GuildSettings",
                "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                "UpdatedAtUTC",
                "GuildSettings",
                "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "CreatedAtUTC",
                "GuildSettings");

            migrationBuilder.DropColumn(
                "UpdatedAtUTC",
                "GuildSettings");

            migrationBuilder.AddColumn<DateTime>(
                "CreatedAtUTC",
                "DiscordChannels",
                "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                "UpdatedAtUTC",
                "DiscordChannels",
                "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
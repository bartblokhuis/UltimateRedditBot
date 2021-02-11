using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations.UltimateDiscordDb
{
    public partial class InitGuildSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Guilds",
                table => new
                {
                    Id = table.Column<decimal>("decimal(20,0)", nullable: false),
                    UpdatedAtUTC = table.Column<DateTime>("datetime2", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>("datetime2", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Guilds", x => x.Id); });

            migrationBuilder.CreateTable(
                "DiscordChannels",
                table => new
                {
                    Id = table.Column<decimal>("decimal(20,0)", nullable: false),
                    GuildId = table.Column<decimal>("decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordChannels", x => x.Id);
                    table.ForeignKey(
                        "FK_DiscordChannels_Guilds_GuildId",
                        x => x.GuildId,
                        "Guilds",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "GuildSettings",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prefix = table.Column<string>("nvarchar(max)", nullable: true),
                    GuildId = table.Column<decimal>("decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildSettings", x => x.Id);
                    table.ForeignKey(
                        "FK_GuildSettings_Guilds_GuildId",
                        x => x.GuildId,
                        "Guilds",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_DiscordChannels_GuildId",
                "DiscordChannels",
                "GuildId");

            migrationBuilder.CreateIndex(
                "IX_GuildSettings_GuildId",
                "GuildSettings",
                "GuildId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "DiscordChannels");

            migrationBuilder.DropTable(
                "GuildSettings");

            migrationBuilder.DropTable(
                "Guilds");
        }
    }
}
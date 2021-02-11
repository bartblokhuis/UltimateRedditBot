using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations.UltimateDiscordDb
{
    public partial class UserSettingsInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Id = table.Column<decimal>("decimal(20,0)", nullable: false),
                    CreatedAtUTC = table.Column<DateTime>("datetime2", nullable: false),
                    UpdatedAtUTC = table.Column<DateTime>("datetime2", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

            migrationBuilder.CreateTable(
                "UserSettings",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<decimal>("decimal(20,0)", nullable: false),
                    Prefix = table.Column<string>("nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                    table.ForeignKey(
                        "FK_UserSettings_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_UserSettings_UserId",
                "UserSettings",
                "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "UserSettings");

            migrationBuilder.DropTable(
                "Users");
        }
    }
}
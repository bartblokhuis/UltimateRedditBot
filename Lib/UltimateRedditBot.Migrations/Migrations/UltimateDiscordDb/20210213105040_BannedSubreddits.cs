using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations.UltimateDiscordDb
{
    public partial class BannedSubreddits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BannedSubreddits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubredditId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannedSubreddits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannedSubreddits_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BannedSubreddits_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BannedSubreddits_GuildId",
                table: "BannedSubreddits",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_BannedSubreddits_UserId",
                table: "BannedSubreddits",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BannedSubreddits");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations.UltimateDiscordDb
{
    public partial class posthistory_init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "PostHistories",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubredditId = table.Column<int>("int", nullable: false),
                    PostId = table.Column<string>("nvarchar(max)", nullable: true),
                    UserId = table.Column<decimal>("decimal(20,0)", nullable: true),
                    GuildId = table.Column<decimal>("decimal(20,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostHistories", x => x.Id);
                    table.ForeignKey(
                        "FK_PostHistories_Guilds_GuildId",
                        x => x.GuildId,
                        "Guilds",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_PostHistories_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_PostHistories_GuildId",
                "PostHistories",
                "GuildId");

            migrationBuilder.CreateIndex(
                "IX_PostHistories_UserId",
                "PostHistories",
                "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "PostHistories");
        }
    }
}
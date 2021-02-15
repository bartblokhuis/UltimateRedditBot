using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations.UltimateDiscordDb
{
    public partial class TextChannelsSubscriptionsInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TextChannelSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextChannelId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextChannelSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextChannelSubscriptions_TextChannels_TextChannelId",
                        column: x => x.TextChannelId,
                        principalTable: "TextChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TextChannelSubscriptions_TextChannelId",
                table: "TextChannelSubscriptions",
                column: "TextChannelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextChannelSubscriptions");
        }
    }
}

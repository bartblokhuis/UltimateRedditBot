using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations.UltimateDiscordDb
{
    public partial class ChangeUlongToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextChannelSubscriptions_TextChannels_TextChannelId1",
                table: "TextChannelSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_TextChannelSubscriptions_TextChannelId1",
                table: "TextChannelSubscriptions");

            migrationBuilder.DropColumn(
                name: "TextChannelId1",
                table: "TextChannelSubscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "TextChannelId",
                table: "TextChannelSubscriptions",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)");

            migrationBuilder.CreateIndex(
                name: "IX_TextChannelSubscriptions_TextChannelId",
                table: "TextChannelSubscriptions",
                column: "TextChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextChannelSubscriptions_TextChannels_TextChannelId",
                table: "TextChannelSubscriptions",
                column: "TextChannelId",
                principalTable: "TextChannels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextChannelSubscriptions_TextChannels_TextChannelId",
                table: "TextChannelSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_TextChannelSubscriptions_TextChannelId",
                table: "TextChannelSubscriptions");

            migrationBuilder.AlterColumn<decimal>(
                name: "TextChannelId",
                table: "TextChannelSubscriptions",
                type: "decimal(20,0)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TextChannelId1",
                table: "TextChannelSubscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TextChannelSubscriptions_TextChannelId1",
                table: "TextChannelSubscriptions",
                column: "TextChannelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TextChannelSubscriptions_TextChannels_TextChannelId1",
                table: "TextChannelSubscriptions",
                column: "TextChannelId1",
                principalTable: "TextChannels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

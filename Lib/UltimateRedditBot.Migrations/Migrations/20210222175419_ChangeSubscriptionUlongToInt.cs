using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations
{
    public partial class ChangeSubscriptionUlongToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Subreddits_SubredditId1",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_SubredditId1",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SubredditId1",
                table: "Subscriptions");

            migrationBuilder.AlterColumn<int>(
                name: "SubredditId",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubredditId",
                table: "Subscriptions",
                column: "SubredditId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Subreddits_SubredditId",
                table: "Subscriptions",
                column: "SubredditId",
                principalTable: "Subreddits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Subreddits_SubredditId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_SubredditId",
                table: "Subscriptions");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubredditId",
                table: "Subscriptions",
                type: "decimal(20,0)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SubredditId1",
                table: "Subscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_SubredditId1",
                table: "Subscriptions",
                column: "SubredditId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Subreddits_SubredditId1",
                table: "Subscriptions",
                column: "SubredditId1",
                principalTable: "Subreddits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

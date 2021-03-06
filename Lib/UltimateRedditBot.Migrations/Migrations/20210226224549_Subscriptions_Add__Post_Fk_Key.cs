using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations
{
    public partial class Subscriptions_Add__Post_Fk_Key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostId",
                table: "Subscriptions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PostId",
                table: "Subscriptions",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Posts_PostId",
                table: "Subscriptions",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Posts_PostId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_PostId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Subscriptions");
        }
    }
}

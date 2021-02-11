using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltimateRedditBot.Migrations.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "GenericSettings",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<string>("nvarchar(max)", nullable: true),
                    KeyGroup = table.Column<string>("nvarchar(max)", nullable: true),
                    Key = table.Column<string>("nvarchar(max)", nullable: true),
                    Value = table.Column<string>("nvarchar(max)", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_GenericSettings", x => x.Id); });

            migrationBuilder.CreateTable(
                "Subreddits",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>("nvarchar(max)", nullable: true),
                    IsNsfw = table.Column<bool>("bit", nullable: false),
                    CreatedAt = table.Column<DateTime>("datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>("datetime2", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Subreddits", x => x.Id); });

            migrationBuilder.CreateTable(
                "Posts",
                table => new
                {
                    Id = table.Column<string>("nvarchar(450)", nullable: false),
                    Author = table.Column<string>("nvarchar(max)", nullable: true),
                    Downs = table.Column<int>("int", nullable: false),
                    Ups = table.Column<int>("int", nullable: false),
                    IsOver18 = table.Column<bool>("bit", nullable: false),
                    Title = table.Column<string>("nvarchar(max)", nullable: true),
                    PostLink = table.Column<string>("nvarchar(max)", nullable: true),
                    Thumbnail = table.Column<string>("nvarchar(max)", nullable: true),
                    Selftext = table.Column<string>("nvarchar(max)", nullable: true),
                    Url = table.Column<string>("nvarchar(max)", nullable: true),
                    SubRedditId = table.Column<int>("int", nullable: false),
                    PostType = table.Column<int>("int", nullable: false),
                    UpdatedAt = table.Column<DateTime>("datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>("datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        "FK_Posts_Subreddits_SubRedditId",
                        x => x.SubRedditId,
                        "Subreddits",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Posts_SubRedditId",
                "Posts",
                "SubRedditId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "GenericSettings");

            migrationBuilder.DropTable(
                "Posts");

            migrationBuilder.DropTable(
                "Subreddits");
        }
    }
}
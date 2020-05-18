using Microsoft.EntityFrameworkCore.Migrations;

namespace AltPro.BackTracker.Migrations
{
    public partial class Comments2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosterId",
                table: "CommentModels");

            migrationBuilder.AddColumn<string>(
                name: "PosterName",
                table: "CommentModels",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosterName",
                table: "CommentModels");

            migrationBuilder.AddColumn<int>(
                name: "PosterId",
                table: "CommentModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

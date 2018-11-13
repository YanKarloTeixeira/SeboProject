using Microsoft.EntityFrameworkCore.Migrations;

namespace SeboProject.Migrations
{
    public partial class Migration07 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoFileName1",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "PhotoFileName2",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "PhotoFileName3",
                table: "Book");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoFileName1",
                table: "Book",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoFileName2",
                table: "Book",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoFileName3",
                table: "Book",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SeboProject.Migrations
{
    public partial class Migration01TableUser_CrediCardName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreditcardName",
                table: "User",
                maxLength: 22,
                nullable: false,
                oldClrType: typeof(double),
                oldMaxLength: 22);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "CreditcardName",
                table: "User",
                maxLength: 22,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 22);
        }
    }
}

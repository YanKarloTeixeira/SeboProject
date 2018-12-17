using Microsoft.EntityFrameworkCore.Migrations;

namespace SeboProject.Migrations
{
    public partial class MigrationOrders2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Order",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Order",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}

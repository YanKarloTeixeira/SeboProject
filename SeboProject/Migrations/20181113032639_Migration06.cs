using Microsoft.EntityFrameworkCore.Migrations;

namespace SeboProject.Migrations
{
    public partial class Migration06 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_User_BuyerUserId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Book_User_SellerId",
                table: "Book");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_BuyerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_SellerId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_BuyerId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_SellerId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Book_BuyerUserId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "BuyerUserId",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "Book",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_SellerId",
                table: "Book",
                newName: "IX_Book_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Order",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_User_UserId",
                table: "Book",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_User_UserId",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Book",
                newName: "SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_Book_UserId",
                table: "Book",
                newName: "IX_Book_SellerId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Order",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "Order",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BuyerUserId",
                table: "Book",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_BuyerId",
                table: "Order",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_SellerId",
                table: "Order",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Book_BuyerUserId",
                table: "Book",
                column: "BuyerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_User_BuyerUserId",
                table: "Book",
                column: "BuyerUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_User_SellerId",
                table: "Book",
                column: "SellerId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_BuyerId",
                table: "Order",
                column: "BuyerId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_SellerId",
                table: "Order",
                column: "SellerId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Cart.API.Migrations
{
    public partial class cartCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_CartCustomers_CartId",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_CartCustomers_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "CartCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_CartCustomers_CartId",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_CartCustomers_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "CartCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

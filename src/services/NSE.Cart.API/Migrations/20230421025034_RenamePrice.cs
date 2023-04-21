using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Cart.API.Migrations
{
    public partial class RenamePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "CartItems");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "CartItems",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartItems");

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "CartItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}

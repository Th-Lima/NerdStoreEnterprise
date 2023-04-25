using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Cart.API.Migrations
{
    public partial class Voucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "CartItems",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalValue",
                table: "CartCustomers",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal");

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "CartCustomers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "VoucherUsed",
                table: "CartCustomers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VoucherCode",
                table: "CartCustomers",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "CartCustomers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeDiscount",
                table: "CartCustomers",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValueDiscount",
                table: "CartCustomers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "CartCustomers");

            migrationBuilder.DropColumn(
                name: "VoucherUsed",
                table: "CartCustomers");

            migrationBuilder.DropColumn(
                name: "VoucherCode",
                table: "CartCustomers");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "CartCustomers");

            migrationBuilder.DropColumn(
                name: "TypeDiscount",
                table: "CartCustomers");

            migrationBuilder.DropColumn(
                name: "ValueDiscount",
                table: "CartCustomers");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "CartItems",
                type: "decimal",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalValue",
                table: "CartCustomers",
                type: "decimal",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}

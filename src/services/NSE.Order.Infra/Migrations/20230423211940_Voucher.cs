using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Order.Infra.Migrations
{
    public partial class Voucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(type: "varchar(100)", nullable: false),
                    Percentage = table.Column<decimal>(nullable: true),
                    ValueDiscount = table.Column<decimal>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    DiscountType = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    UtilizationDate = table.Column<DateTime>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Used = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vouchers");
        }
    }
}

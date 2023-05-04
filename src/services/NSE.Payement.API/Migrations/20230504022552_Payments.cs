using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Payment.API.Migrations
{
    public partial class Payments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    TypePayment = table.Column<int>(nullable: false),
                    TotalValue = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AuthorizationCode = table.Column<string>(type: "varchar(100)", nullable: true),
                    CardBrand = table.Column<string>(type: "varchar(100)", nullable: true),
                    TransactionDate = table.Column<DateTime>(nullable: true),
                    TotalValue = table.Column<decimal>(nullable: false),
                    TransactionCost = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TID = table.Column<string>(type: "varchar(100)", nullable: true),
                    NSU = table.Column<string>(type: "varchar(100)", nullable: true),
                    PaymentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PaymentId",
                table: "Transactions",
                column: "PaymentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}

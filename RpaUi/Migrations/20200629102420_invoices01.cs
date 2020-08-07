using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class invoices01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                table: "tblPayments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tblPayments_InvoiceId",
                table: "tblPayments",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblPayments_tblInvoices_InvoiceId",
                table: "tblPayments",
                column: "InvoiceId",
                principalTable: "tblInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblPayments_tblInvoices_InvoiceId",
                table: "tblPayments");

            migrationBuilder.DropIndex(
                name: "IX_tblPayments_InvoiceId",
                table: "tblPayments");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "tblPayments");
        }
    }
}

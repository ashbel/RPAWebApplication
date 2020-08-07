using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class attachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "invoiceAttachment",
                table: "tblInvoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "communicationAttachment",
                table: "tblCommunications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "invoiceAttachment",
                table: "tblInvoices");

            migrationBuilder.DropColumn(
                name: "communicationAttachment",
                table: "tblCommunications");
        }
    }
}

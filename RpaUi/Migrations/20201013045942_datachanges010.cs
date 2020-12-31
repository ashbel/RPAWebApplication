using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class datachanges010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblPayments_tblPharmacists_tblPharmacistsId",
                table: "tblPayments");

            migrationBuilder.AddForeignKey(
                name: "FK_tblPayments_tblPharmacists_tblPharmacistsId",
                table: "tblPayments",
                column: "tblPharmacistsId",
                principalTable: "tblPharmacists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblPayments_tblPharmacists_tblPharmacistsId",
                table: "tblPayments");

            migrationBuilder.AddForeignKey(
                name: "FK_tblPayments_tblPharmacists_tblPharmacistsId",
                table: "tblPayments",
                column: "tblPharmacistsId",
                principalTable: "tblPharmacists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

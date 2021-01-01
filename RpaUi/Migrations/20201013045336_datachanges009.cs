using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class datachanges009 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblPayments_AspNetUsers_ClientId",
                table: "tblPayments");

            migrationBuilder.DropIndex(
                name: "IX_tblPayments_ClientId",
                table: "tblPayments");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "tblPayments");

            migrationBuilder.AddColumn<int>(
                name: "tblPharmacistsId",
                table: "tblPayments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tblPharmacistsId",
                table: "tblPayments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tblPayments_tblPharmacistsId",
                table: "tblPayments",
                column: "tblPharmacistsId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblPayments_tblPharmacists_tblPharmacistsId",
                table: "tblPayments",
                column: "tblPharmacistsId",
                principalTable: "tblPharmacists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblPayments_tblPharmacists_tblPharmacistsId",
                table: "tblPayments");

            migrationBuilder.DropIndex(
                name: "IX_tblPayments_tblPharmacistsId",
                table: "tblPayments");

            migrationBuilder.DropColumn(
                name: "tblPharmacistsId",
                table: "tblPayments");

            migrationBuilder.DropColumn(
                name: "tblPharmacistsId",
                table: "tblPayments");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "tblPayments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_tblPayments_ClientId",
                table: "tblPayments",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblPayments_AspNetUsers_ClientId",
                table: "tblPayments",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

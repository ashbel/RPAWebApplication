using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class DataChangesClientIdAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "tblPharmacists",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_tblPharmacists_ClientId",
                table: "tblPharmacists",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblPharmacists_AspNetUsers_ClientId",
                table: "tblPharmacists",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblPharmacists_AspNetUsers_ClientId",
                table: "tblPharmacists");

            migrationBuilder.DropIndex(
                name: "IX_tblPharmacists_ClientId",
                table: "tblPharmacists");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "tblPharmacists");
        }
    }
}

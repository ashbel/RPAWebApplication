using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class DataChangesClientIdRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblPharmacists_AspNetUsers_ClientId1",
                table: "tblPharmacists");

            migrationBuilder.DropIndex(
                name: "IX_tblPharmacists_ClientId1",
                table: "tblPharmacists");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "tblPharmacists");

            migrationBuilder.DropColumn(
                name: "ClientId1",
                table: "tblPharmacists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "tblPharmacists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClientId1",
                table: "tblPharmacists",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblPharmacists_ClientId1",
                table: "tblPharmacists",
                column: "ClientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_tblPharmacists_AspNetUsers_ClientId1",
                table: "tblPharmacists",
                column: "ClientId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

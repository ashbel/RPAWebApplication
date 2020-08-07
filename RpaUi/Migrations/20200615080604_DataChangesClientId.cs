using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class DataChangesClientId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblPharmacists_AspNetUsers_ClientId",
                table: "tblPharmacists");

            migrationBuilder.DropIndex(
                name: "IX_tblPharmacists_ClientId",
                table: "tblPharmacists");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "tblPharmacists",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId1",
                table: "tblPharmacists",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblPharmacists_AspNetUsers_ClientId1",
                table: "tblPharmacists");

            migrationBuilder.DropIndex(
                name: "IX_tblPharmacists_ClientId1",
                table: "tblPharmacists");

            migrationBuilder.DropColumn(
                name: "ClientId1",
                table: "tblPharmacists");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "tblPharmacists",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int));

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
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class datachangesRenaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEventsHistory_AspNetUsers_ClientId",
                table: "tblEventsHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_tblPharmacists_AspNetUsers_ClientId",
                table: "tblPharmacists");

            migrationBuilder.DropIndex(
                name: "IX_tblPharmacists_ClientId",
                table: "tblPharmacists");

            migrationBuilder.DropIndex(
                name: "IX_tblEventsHistory_ClientId",
                table: "tblEventsHistory");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "tblPharmacists");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "tblEventsHistory");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "tblPharmacists",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "tblPharmacistsId",
                table: "tblEventsHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EventComplete",
                table: "tblEvents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_tblPharmacists_ApplicationUserId",
                table: "tblPharmacists",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEventsHistory_tblPharmacistsId",
                table: "tblEventsHistory",
                column: "tblPharmacistsId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblEventsHistory_tblPharmacists_tblPharmacistsId",
                table: "tblEventsHistory",
                column: "tblPharmacistsId",
                principalTable: "tblPharmacists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblPharmacists_AspNetUsers_ApplicationUserId",
                table: "tblPharmacists",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblEventsHistory_tblPharmacists_tblPharmacistsId",
                table: "tblEventsHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_tblPharmacists_AspNetUsers_ApplicationUserId",
                table: "tblPharmacists");

            migrationBuilder.DropIndex(
                name: "IX_tblPharmacists_ApplicationUserId",
                table: "tblPharmacists");

            migrationBuilder.DropIndex(
                name: "IX_tblEventsHistory_tblPharmacistsId",
                table: "tblEventsHistory");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "tblPharmacists");

            migrationBuilder.DropColumn(
                name: "tblPharmacistsId",
                table: "tblEventsHistory");

            migrationBuilder.DropColumn(
                name: "EventComplete",
                table: "tblEvents");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "tblPharmacists",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "tblEventsHistory",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_tblPharmacists_ClientId",
                table: "tblPharmacists",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEventsHistory_ClientId",
                table: "tblEventsHistory",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblEventsHistory_AspNetUsers_ClientId",
                table: "tblEventsHistory",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblPharmacists_AspNetUsers_ClientId",
                table: "tblPharmacists",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class _3332 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblCertificates_AspNetUsers_ClientId",
                table: "tblCertificates");

            migrationBuilder.DropIndex(
                name: "IX_tblCertificates_ClientId",
                table: "tblCertificates");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "tblCertificates");

            migrationBuilder.AddColumn<int>(
                name: "tblResourceCategoryId",
                table: "tblResources",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "tblPharmacistsId",
                table: "tblCertificates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tblResourceCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    catName = table.Column<string>(nullable: false),
                    catDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblResourceCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblResources_tblResourceCategoryId",
                table: "tblResources",
                column: "tblResourceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_tblCertificates_tblPharmacistsId",
                table: "tblCertificates",
                column: "tblPharmacistsId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblCertificates_tblPharmacists_tblPharmacistsId",
                table: "tblCertificates",
                column: "tblPharmacistsId",
                principalTable: "tblPharmacists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblResources_tblResourceCategory_tblResourceCategoryId",
                table: "tblResources",
                column: "tblResourceCategoryId",
                principalTable: "tblResourceCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblCertificates_tblPharmacists_tblPharmacistsId",
                table: "tblCertificates");

            migrationBuilder.DropForeignKey(
                name: "FK_tblResources_tblResourceCategory_tblResourceCategoryId",
                table: "tblResources");

            migrationBuilder.DropTable(
                name: "tblResourceCategory");

            migrationBuilder.DropIndex(
                name: "IX_tblResources_tblResourceCategoryId",
                table: "tblResources");

            migrationBuilder.DropIndex(
                name: "IX_tblCertificates_tblPharmacistsId",
                table: "tblCertificates");

            migrationBuilder.DropColumn(
                name: "tblResourceCategoryId",
                table: "tblResources");

            migrationBuilder.DropColumn(
                name: "tblPharmacistsId",
                table: "tblCertificates");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "tblCertificates",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_tblCertificates_ClientId",
                table: "tblCertificates",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblCertificates_AspNetUsers_ClientId",
                table: "tblCertificates",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class tblInvoiceClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblInvoiceClient",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    tblInvoicesId = table.Column<int>(nullable: false),
                    tblPharmacistsId = table.Column<int>(nullable: false),
                    paid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInvoiceClient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblInvoiceClient_tblInvoices_tblInvoicesId",
                        column: x => x.tblInvoicesId,
                        principalTable: "tblInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblInvoiceClient_tblPharmacists_tblPharmacistsId",
                        column: x => x.tblPharmacistsId,
                        principalTable: "tblPharmacists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblInvoiceClient_tblInvoicesId",
                table: "tblInvoiceClient",
                column: "tblInvoicesId");

            migrationBuilder.CreateIndex(
                name: "IX_tblInvoiceClient_tblPharmacistsId",
                table: "tblInvoiceClient",
                column: "tblPharmacistsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblInvoiceClient");
        }
    }
}

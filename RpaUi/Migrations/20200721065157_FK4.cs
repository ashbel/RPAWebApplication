using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class FK4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "dateOfRenewal",
                table: "tblPharmacists",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "tblMailingList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    ListName = table.Column<string>(nullable: true),
                    ListDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMailingList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblMailingListClients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    tblMailingListId = table.Column<int>(nullable: false),
                    tblPharmacistId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMailingListClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblMailingListClients_tblMailingList_tblMailingListId",
                        column: x => x.tblMailingListId,
                        principalTable: "tblMailingList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblMailingListClients_tblPharmacists_tblPharmacistId",
                        column: x => x.tblPharmacistId,
                        principalTable: "tblPharmacists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblMailingListClients_tblMailingListId",
                table: "tblMailingListClients",
                column: "tblMailingListId");

            migrationBuilder.CreateIndex(
                name: "IX_tblMailingListClients_tblPharmacistId",
                table: "tblMailingListClients",
                column: "tblPharmacistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblMailingListClients");

            migrationBuilder.DropTable(
                name: "tblMailingList");

            migrationBuilder.DropColumn(
                name: "dateOfRenewal",
                table: "tblPharmacists");
        }
    }
}

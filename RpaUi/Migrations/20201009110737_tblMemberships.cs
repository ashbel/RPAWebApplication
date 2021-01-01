using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class tblMemberships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblMembership",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMembership", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblMembershipClients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    tblMembershipId = table.Column<int>(nullable: false),
                    tblPharmacistsId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMembershipClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblMembershipClients_tblMembership_tblMembershipId",
                        column: x => x.tblMembershipId,
                        principalTable: "tblMembership",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblMembershipClients_tblPharmacists_tblPharmacistsId",
                        column: x => x.tblPharmacistsId,
                        principalTable: "tblPharmacists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblMembershipClients_tblMembershipId",
                table: "tblMembershipClients",
                column: "tblMembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_tblMembershipClients_tblPharmacistsId",
                table: "tblMembershipClients",
                column: "tblPharmacistsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblMembershipClients");

            migrationBuilder.DropTable(
                name: "tblMembership");
        }
    }
}

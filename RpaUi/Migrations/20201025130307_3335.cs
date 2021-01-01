using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class _3335 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblResourcesMembers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    tblResourcesId = table.Column<int>(nullable: false),
                    tblMembershipId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblResourcesMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblResourcesMembers_tblMembership_tblMembershipId",
                        column: x => x.tblMembershipId,
                        principalTable: "tblMembership",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblResourcesMembers_tblResources_tblResourcesId",
                        column: x => x.tblResourcesId,
                        principalTable: "tblResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblResourcesMembers_tblMembershipId",
                table: "tblResourcesMembers",
                column: "tblMembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_tblResourcesMembers_tblResourcesId",
                table: "tblResourcesMembers",
                column: "tblResourcesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblResourcesMembers");
        }
    }
}

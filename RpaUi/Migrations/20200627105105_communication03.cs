using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class communication03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblCommunicationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    CommunicationId = table.Column<int>(nullable: false),
                    Receipient = table.Column<string>(nullable: true),
                    ClientId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCommunicationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblCommunicationLogs_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblCommunicationLogs_tblCommunications_CommunicationId",
                        column: x => x.CommunicationId,
                        principalTable: "tblCommunications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblCommunicationLogs_ClientId",
                table: "tblCommunicationLogs",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_tblCommunicationLogs_CommunicationId",
                table: "tblCommunicationLogs",
                column: "CommunicationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblCommunicationLogs");
        }
    }
}

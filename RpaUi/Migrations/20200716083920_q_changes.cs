using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class q_changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "tblPharmacists",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "tblQualifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    qName = table.Column<string>(nullable: true),
                    qDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblQualifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblQualifications_Pharmacists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    PharmacistId = table.Column<int>(nullable: false),
                    QualificationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblQualifications_Pharmacists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblQualifications_Pharmacists_tblPharmacists_PharmacistId",
                        column: x => x.PharmacistId,
                        principalTable: "tblPharmacists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblQualifications_Pharmacists_tblQualifications_QualificationId",
                        column: x => x.QualificationId,
                        principalTable: "tblQualifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblQualifications_Pharmacists_PharmacistId",
                table: "tblQualifications_Pharmacists",
                column: "PharmacistId");

            migrationBuilder.CreateIndex(
                name: "IX_tblQualifications_Pharmacists_QualificationId",
                table: "tblQualifications_Pharmacists",
                column: "QualificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblQualifications_Pharmacists");

            migrationBuilder.DropTable(
                name: "tblQualifications");

            migrationBuilder.DropColumn(
                name: "status",
                table: "tblPharmacists");
        }
    }
}

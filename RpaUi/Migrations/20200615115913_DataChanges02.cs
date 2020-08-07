using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class DataChanges02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblPayments_tblPaymentTypes_PaymentTypeId",
                table: "tblPayments");

            migrationBuilder.DropTable(
                name: "tblPaymentTypes");

            migrationBuilder.CreateTable(
                name: "tblCodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    CodeName = table.Column<string>(nullable: true),
                    CodeEntity = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCodes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_tblPayments_tblCodes_PaymentTypeId",
                table: "tblPayments",
                column: "PaymentTypeId",
                principalTable: "tblCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblPayments_tblCodes_PaymentTypeId",
                table: "tblPayments");

            migrationBuilder.DropTable(
                name: "tblCodes");

            migrationBuilder.CreateTable(
                name: "tblPaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPaymentTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_tblPayments_tblPaymentTypes_PaymentTypeId",
                table: "tblPayments",
                column: "PaymentTypeId",
                principalTable: "tblPaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

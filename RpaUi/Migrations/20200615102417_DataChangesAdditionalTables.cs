using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class DataChangesAdditionalTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblCertificates_AspNetUsers_ClientId",
                table: "tblCertificates");

            migrationBuilder.DropForeignKey(
                name: "FK_tblPayments_AspNetUsers_ClientId",
                table: "tblPayments");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "tblPayments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountPaid",
                table: "tblPayments",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PayDate",
                table: "tblPayments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PaymentComment",
                table: "tblPayments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentTypeId",
                table: "tblPayments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "EventVenue",
                table: "tblEvents",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EventName",
                table: "tblEvents",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EventDescription",
                table: "tblEvents",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "EventPoints",
                table: "tblEvents",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "tblCertificates",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CertificateDate",
                table: "tblCertificates",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "tblCertificates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "EventPoints",
                table: "tblCertificates",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "tblEventsHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    ClientId = table.Column<string>(nullable: false),
                    EventId = table.Column<int>(nullable: false),
                    AttendedEvent = table.Column<bool>(nullable: false),
                    EventHistoryComment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEventsHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblEventsHistory_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblEventsHistory_tblEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "tblEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblPaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    PaymentTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblPayments_PaymentTypeId",
                table: "tblPayments",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblCertificates_EventId",
                table: "tblCertificates",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEventsHistory_ClientId",
                table: "tblEventsHistory",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_tblEventsHistory_EventId",
                table: "tblEventsHistory",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblCertificates_AspNetUsers_ClientId",
                table: "tblCertificates",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblCertificates_tblEvents_EventId",
                table: "tblCertificates",
                column: "EventId",
                principalTable: "tblEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblPayments_AspNetUsers_ClientId",
                table: "tblPayments",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tblPayments_tblPaymentTypes_PaymentTypeId",
                table: "tblPayments",
                column: "PaymentTypeId",
                principalTable: "tblPaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblCertificates_AspNetUsers_ClientId",
                table: "tblCertificates");

            migrationBuilder.DropForeignKey(
                name: "FK_tblCertificates_tblEvents_EventId",
                table: "tblCertificates");

            migrationBuilder.DropForeignKey(
                name: "FK_tblPayments_AspNetUsers_ClientId",
                table: "tblPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_tblPayments_tblPaymentTypes_PaymentTypeId",
                table: "tblPayments");

            migrationBuilder.DropTable(
                name: "tblEventsHistory");

            migrationBuilder.DropTable(
                name: "tblPaymentTypes");

            migrationBuilder.DropIndex(
                name: "IX_tblPayments_PaymentTypeId",
                table: "tblPayments");

            migrationBuilder.DropIndex(
                name: "IX_tblCertificates_EventId",
                table: "tblCertificates");

            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "tblPayments");

            migrationBuilder.DropColumn(
                name: "PayDate",
                table: "tblPayments");

            migrationBuilder.DropColumn(
                name: "PaymentComment",
                table: "tblPayments");

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "tblPayments");

            migrationBuilder.DropColumn(
                name: "EventDescription",
                table: "tblEvents");

            migrationBuilder.DropColumn(
                name: "EventPoints",
                table: "tblEvents");

            migrationBuilder.DropColumn(
                name: "CertificateDate",
                table: "tblCertificates");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "tblCertificates");

            migrationBuilder.DropColumn(
                name: "EventPoints",
                table: "tblCertificates");

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "tblPayments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "EventVenue",
                table: "tblEvents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "EventName",
                table: "tblEvents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ClientId",
                table: "tblCertificates",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_tblCertificates_AspNetUsers_ClientId",
                table: "tblCertificates",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblPayments_AspNetUsers_ClientId",
                table: "tblPayments",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

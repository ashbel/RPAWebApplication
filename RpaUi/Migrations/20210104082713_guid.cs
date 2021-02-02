using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class guid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblSubscriptions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblResourcesMembers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblResources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblResourceCategory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblQualifications_Pharmacists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblQualifications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblPharmacists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblPayments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblMembershipClients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblMembership",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblMailingListClients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblMailingList",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblJobs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblInvoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblInvoiceClient",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblEventsHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblEvents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblEmails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblCommunications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblCommunicationLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblCodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "tblCertificates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblSubscriptions");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblResourcesMembers");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblResources");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblResourceCategory");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblQualifications_Pharmacists");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblQualifications");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblPharmacists");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblPayments");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblMembershipClients");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblMembership");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblMailingListClients");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblMailingList");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblJobs");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblInvoices");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblInvoiceClient");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblEventsHistory");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblEvents");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblEmails");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblDocuments");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblCommunications");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblCommunicationLogs");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblCodes");

            migrationBuilder.DropColumn(
                name: "Guid",
                table: "tblCertificates");
        }
    }
}

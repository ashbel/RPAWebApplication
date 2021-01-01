using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class changes333 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "otherQualification",
                table: "tblPharmacists",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventSponsor",
                table: "tblEvents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "otherQualification",
                table: "tblPharmacists");

            migrationBuilder.DropColumn(
                name: "EventSponsor",
                table: "tblEvents");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class pharmatable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pharmacyLogo",
                table: "tblPharmacies");

            migrationBuilder.AlterColumn<string>(
                name: "pharmacyPhone",
                table: "tblPharmacies",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "pharmacyPhone",
                table: "tblPharmacies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pharmacyLogo",
                table: "tblPharmacies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

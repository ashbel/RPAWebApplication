using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class EmailConfigs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblEmails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(nullable: false),
                    email_smtp = table.Column<string>(nullable: false),
                    email_address = table.Column<string>(nullable: false),
                    email_port = table.Column<string>(nullable: false),
                    email_username = table.Column<string>(nullable: false),
                    email_password = table.Column<string>(nullable: false),
                    email_ssl = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEmails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblEmails");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class DataChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "created",
                table: "tblSubscriptions",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tblSubscriptions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "tblPharmacists",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tblPharmacists",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "tblPayments",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tblPayments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "tblEvents",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tblEvents",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "tblDocuments",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tblDocuments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "tblCertificates",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tblCertificates",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "tblPharmacists",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "tblPayments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "tblDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "tblCertificates",
                nullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Name",
            //    table: "AspNetUserTokens",
            //    maxLength: 128,
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(450)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "LoginProvider",
            //    table: "AspNetUserTokens",
            //    maxLength: 128,
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(450)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "ProviderKey",
            //    table: "AspNetUserLogins",
            //    maxLength: 128,
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(450)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "LoginProvider",
            //    table: "AspNetUserLogins",
            //    maxLength: 128,
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_tblPharmacists_ClientId",
                table: "tblPharmacists",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_tblPayments_ClientId",
                table: "tblPayments",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_tblDocuments_ClientId",
                table: "tblDocuments",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_tblCertificates_ClientId",
                table: "tblCertificates",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblCertificates_AspNetUsers_ClientId",
                table: "tblCertificates",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblDocuments_AspNetUsers_ClientId",
                table: "tblDocuments",
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

            migrationBuilder.AddForeignKey(
                name: "FK_tblPharmacists_AspNetUsers_ClientId",
                table: "tblPharmacists",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblCertificates_AspNetUsers_ClientId",
                table: "tblCertificates");

            migrationBuilder.DropForeignKey(
                name: "FK_tblDocuments_AspNetUsers_ClientId",
                table: "tblDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_tblPayments_AspNetUsers_ClientId",
                table: "tblPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_tblPharmacists_AspNetUsers_ClientId",
                table: "tblPharmacists");

            migrationBuilder.DropIndex(
                name: "IX_tblPharmacists_ClientId",
                table: "tblPharmacists");

            migrationBuilder.DropIndex(
                name: "IX_tblPayments_ClientId",
                table: "tblPayments");

            migrationBuilder.DropIndex(
                name: "IX_tblDocuments_ClientId",
                table: "tblDocuments");

            migrationBuilder.DropIndex(
                name: "IX_tblCertificates_ClientId",
                table: "tblCertificates");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "tblPharmacists");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "tblPayments");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "tblDocuments");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "tblCertificates");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "tblSubscriptions",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tblSubscriptions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "tblPharmacists",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tblPharmacists",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "tblPayments",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tblPayments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "tblEvents",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tblEvents",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "tblDocuments",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tblDocuments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "tblCertificates",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tblCertificates",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);
        }
    }
}

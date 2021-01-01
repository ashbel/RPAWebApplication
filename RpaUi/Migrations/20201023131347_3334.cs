using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class _3334 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblResources_tblResourceCategories_tblResourceCategoryId",
                table: "tblResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblResourceCategories",
                table: "tblResourceCategories");

            migrationBuilder.RenameTable(
                name: "tblResourceCategories",
                newName: "tblResourceCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblResourceCategory",
                table: "tblResourceCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblResources_tblResourceCategory_tblResourceCategoryId",
                table: "tblResources",
                column: "tblResourceCategoryId",
                principalTable: "tblResourceCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblResources_tblResourceCategory_tblResourceCategoryId",
                table: "tblResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tblResourceCategory",
                table: "tblResourceCategory");

            migrationBuilder.RenameTable(
                name: "tblResourceCategory",
                newName: "tblResourceCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblResourceCategories",
                table: "tblResourceCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tblResources_tblResourceCategories_tblResourceCategoryId",
                table: "tblResources",
                column: "tblResourceCategoryId",
                principalTable: "tblResourceCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblEventsHistory",
                table: "tblEventsHistory");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "tblEventsHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblEventsHistory",
                table: "tblEventsHistory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_tblEventsHistory_EventId",
                table: "tblEventsHistory",
                column: "EventId");
        }
    }
}

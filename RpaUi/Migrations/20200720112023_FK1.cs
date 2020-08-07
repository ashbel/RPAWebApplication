using Microsoft.EntityFrameworkCore.Migrations;

namespace RpaUi.Migrations
{
    public partial class FK1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblEventsHistory",
                table: "tblEventsHistory");

            migrationBuilder.DropIndex(
                name: "IX_tblEventsHistory_EventId",
                table: "tblEventsHistory");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "tblEventsHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblEventsHistory",
                table: "tblEventsHistory",
                column: "EventId");
        }
    }
}

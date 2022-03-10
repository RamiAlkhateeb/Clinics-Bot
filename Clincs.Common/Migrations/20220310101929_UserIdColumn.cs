using Microsoft.EntityFrameworkCore.Migrations;

namespace Clincs.Common.Migrations
{
    public partial class UserIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "notificationEntities",
                newName: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "notificationEntities",
                newName: "ActivityId");
        }
    }
}

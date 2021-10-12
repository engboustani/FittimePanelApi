using Microsoft.EntityFrameworkCore.Migrations;

namespace FittimePanelApi.Migrations
{
    public partial class roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ba8145e0-1944-4ae2-8275-e3d23bdca367", "f4138331-0573-4d76-8203-3730896c6840", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a8277e51-fc3c-49bb-a550-92fe9f5c6308", "e1ce438b-99ac-4f91-ad4c-3a2bceea1da3", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8277e51-fc3c-49bb-a550-92fe9f5c6308");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba8145e0-1944-4ae2-8275-e3d23bdca367");
        }
    }
}

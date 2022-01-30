using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FittimePanelApi.Migrations
{
    public partial class updateexercisetype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f65c578-0ffe-4fef-9b74-8643fc817176");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "488502dc-758f-46b8-9f24-528f892c8765");

            migrationBuilder.AlterColumn<uint>(
                name: "Price",
                table: "ExerciseTypes",
                type: "int unsigned",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.InsertData(
                table: "ExerciseTypes",
                columns: new[] { "Id", "CreatedDate", "Name", "Price", "UpdatedDate" },
                values: new object[] { 1, new DateTime(2021, 12, 7, 9, 20, 52, 563, DateTimeKind.Local).AddTicks(1832), "تمرین بدن سازی", 2000000u, new DateTime(2021, 12, 7, 9, 20, 52, 563, DateTimeKind.Local).AddTicks(1929) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5c8df1f2-f0dc-4994-8696-645100ba5654");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3a5905c-9ff5-4e8a-91ea-7015531a994a");

            migrationBuilder.DeleteData(
                table: "ExerciseTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "ExerciseTypes",
                type: "double",
                nullable: false,
                oldClrType: typeof(uint),
                oldType: "int unsigned");
        }
    }
}

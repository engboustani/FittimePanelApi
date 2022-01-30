using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FittimePanelApi.Migrations
{
    public partial class updateexercisetypeprop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5c8df1f2-f0dc-4994-8696-645100ba5654");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3a5905c-9ff5-4e8a-91ea-7015531a994a");

            migrationBuilder.UpdateData(
                table: "ExerciseTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 12, 7, 11, 24, 38, 433, DateTimeKind.Local).AddTicks(7520), new DateTime(2021, 12, 7, 11, 24, 38, 433, DateTimeKind.Local).AddTicks(7648) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6257cb2e-57b2-4c80-b358-150555ee407c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa6718e2-edda-4a8e-b1fb-9903bdc76b86");

            migrationBuilder.UpdateData(
                table: "ExerciseTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2021, 12, 7, 9, 20, 52, 563, DateTimeKind.Local).AddTicks(1832), new DateTime(2021, 12, 7, 9, 20, 52, 563, DateTimeKind.Local).AddTicks(1929) });
        }
    }
}

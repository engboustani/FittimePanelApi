using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FittimePanelApi.Migrations
{
    public partial class addfieldpayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Payments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NameFa",
                table: "PaymentGetways",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "NameFa",
                table: "PaymentGetways");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FittimePanelApi.Migrations
{
    public partial class paymentDiscount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DiscountId",
                table: "Payments",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "PaymentDiscount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Percentage = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<double>(type: "double", nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    Limited = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Limit = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDiscount", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DiscountId",
                table: "Payments",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentDiscount_DiscountId",
                table: "Payments",
                column: "DiscountId",
                principalTable: "PaymentDiscount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseDownloads_Exercises_ExerciseId",
                table: "ExerciseDownloads");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentDiscount_DiscountId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "PaymentDiscount");

            migrationBuilder.DropIndex(
                name: "IX_Payments_DiscountId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Payments");
        }
    }
}

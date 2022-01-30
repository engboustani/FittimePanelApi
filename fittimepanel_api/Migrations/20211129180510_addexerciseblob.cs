using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FittimePanelApi.Migrations
{
    public partial class addexerciseblob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12600347-412a-4106-9ca1-7d23922c076f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5b6c028-9c10-4a26-8024-09f46edb1f69");

            migrationBuilder.CreateTable(
                name: "ExerciseBlobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Key = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<byte[]>(type: "MediumBlob", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExerciseId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseBlobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseBlobs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExerciseBlobs_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1f65c578-0ffe-4fef-9b74-8643fc817176", "8448d930-7acc-439e-80ba-f6d36d6abe4c", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "488502dc-758f-46b8-9f24-528f892c8765", "af55d428-676f-4804-8eb4-7c8c23788ea6", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseBlobs_ExerciseId",
                table: "ExerciseBlobs",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseBlobs_UserId",
                table: "ExerciseBlobs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseBlobs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f65c578-0ffe-4fef-9b74-8643fc817176");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "488502dc-758f-46b8-9f24-528f892c8765");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f5b6c028-9c10-4a26-8024-09f46edb1f69", "06b3471c-e94e-4bab-9d22-27717d070ddc", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "12600347-412a-4106-9ca1-7d23922c076f", "92166011-1e98-420e-a2fc-577d47bc3e62", "Administrator", "ADMINISTRATOR" });
        }
    }
}

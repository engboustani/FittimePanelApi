using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FittimePanelApi.Migrations
{
    public partial class updateExcersiesProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ExerciseMetas",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ExerciseDownloads",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<byte[]>(
                name: "Value",
                table: "ExerciseDownloads",
                type: "MediumBlob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ExerciseBlobs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ExerciseMetas");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ExerciseDownloads");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "ExerciseDownloads");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ExerciseBlobs");
        }
    }
}

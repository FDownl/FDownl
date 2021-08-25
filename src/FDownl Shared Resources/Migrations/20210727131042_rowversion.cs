using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FDownl_Shared_Resources.Migrations
{
    public partial class rowversion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFull",
                table: "StorageServers");

            migrationBuilder.AddColumn<DateTime>(
                name: "RowVersion",
                table: "UploadedFiles",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "RowVersion",
                table: "StorageServers",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "UploadedFiles");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "StorageServers");

            migrationBuilder.AddColumn<bool>(
                name: "IsFull",
                table: "StorageServers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}

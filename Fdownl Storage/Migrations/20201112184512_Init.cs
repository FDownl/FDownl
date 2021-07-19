using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fdownl_Storage.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StorageServers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ip = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Hostname = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Location = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    IsFull = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageServers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploadedFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RandomId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ServerName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Hostname = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Filename = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Downloads = table.Column<int>(type: "int", nullable: false),
                    MaxDownloads = table.Column<int>(type: "int", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Lifetime = table.Column<int>(type: "int", nullable: false),
                    Ip = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFiles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StorageServers");

            migrationBuilder.DropTable(
                name: "UploadedFiles");
        }
    }
}

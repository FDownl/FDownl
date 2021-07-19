using Microsoft.EntityFrameworkCore.Migrations;

namespace Fdownl_Storage.Migrations
{
    public partial class Coupon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Coupon",
                table: "UploadedFiles",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coupon",
                table: "UploadedFiles");
        }
    }
}

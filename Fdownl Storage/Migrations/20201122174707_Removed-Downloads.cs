using Microsoft.EntityFrameworkCore.Migrations;

namespace Fdownl_Storage.Migrations
{
    public partial class RemovedDownloads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Downloads",
                table: "UploadedFiles");

            migrationBuilder.DropColumn(
                name: "MaxDownloads",
                table: "UploadedFiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Downloads",
                table: "UploadedFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxDownloads",
                table: "UploadedFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

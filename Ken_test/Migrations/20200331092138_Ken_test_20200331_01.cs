using Microsoft.EntityFrameworkCore.Migrations;

namespace Ken_test.Migrations
{
    public partial class Ken_test_20200331_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeadPicture",
                table: "user_info",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "user_info",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadPicture",
                table: "user_info");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "user_info");
        }
    }
}

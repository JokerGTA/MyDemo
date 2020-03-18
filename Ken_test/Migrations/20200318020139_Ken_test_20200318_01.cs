using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ken_test.Migrations
{
    public partial class Ken_test_20200318_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_info",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EditTime = table.Column<DateTime>(type: "DateTime", nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    NickName = table.Column<string>(maxLength: 50, nullable: true),
                    RealName = table.Column<string>(maxLength: 50, nullable: true),
                    Password = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_info", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "message_log",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EditTime = table.Column<DateTime>(type: "DateTime", nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    MsgContext = table.Column<string>(maxLength: 500, nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_message_log", x => x.Id);
                    table.ForeignKey(
                        name: "FK_message_log_user_info_UserId",
                        column: x => x.UserId,
                        principalTable: "user_info",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_message_log_UserId",
                table: "message_log",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "message_log");

            migrationBuilder.DropTable(
                name: "user_info");
        }
    }
}

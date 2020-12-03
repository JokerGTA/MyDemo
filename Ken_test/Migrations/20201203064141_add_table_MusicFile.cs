using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ken_test.Migrations
{
    public partial class add_table_MusicFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ken_music_file",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EditTime = table.Column<DateTime>(type: "DateTime", nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    CoverUrl = table.Column<string>(maxLength: 200, nullable: true),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ken_music_file", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ken_music",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EditTime = table.Column<DateTime>(type: "DateTime", nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    Url = table.Column<string>(maxLength: 200, nullable: true),
                    CoverImgUrl = table.Column<string>(maxLength: 500, nullable: true),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    MusicFileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ken_music", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ken_music_ken_music_file_MusicFileId",
                        column: x => x.MusicFileId,
                        principalTable: "ken_music_file",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ken_music_MusicFileId",
                table: "ken_music",
                column: "MusicFileId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ken_music");

            migrationBuilder.DropTable(
                name: "ken_music_file");
        }
    }
}

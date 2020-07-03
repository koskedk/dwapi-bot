using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dwapi.Bot.Infrastructure.Migrations
{
    public partial class BlockStagez : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlockStages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    BlockCount = table.Column<int>(nullable: false),
                    Completed = table.Column<int>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockStages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockStages");
        }
    }
}

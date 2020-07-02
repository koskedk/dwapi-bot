using Microsoft.EntityFrameworkCore.Migrations;

namespace Dwapi.Bot.Infrastructure.Migrations
{
    public partial class BlockStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InterSiteBlockStatus",
                table: "SubjectIndices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SiteBlockStatus",
                table: "SubjectIndices",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterSiteBlockStatus",
                table: "SubjectIndices");

            migrationBuilder.DropColumn(
                name: "SiteBlockStatus",
                table: "SubjectIndices");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dwapi.Bot.Infrastructure.Migrations
{
    public partial class BlockReveiw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InterSiteBlockId",
                table: "SubjectIndices",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SiteBlockId",
                table: "SubjectIndices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterSiteBlockId",
                table: "SubjectIndices");

            migrationBuilder.DropColumn(
                name: "SiteBlockId",
                table: "SubjectIndices");
        }
    }
}

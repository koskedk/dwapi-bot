using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dwapi.Bot.Infrastructure.Migrations
{
    public partial class BotInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientIndices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PatientPk = table.Column<int>(nullable: false),
                    SiteCode = table.Column<int>(nullable: false),
                    FacilityName = table.Column<string>(nullable: true),
                    Serial = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: false),
                    PatientID = table.Column<string>(nullable: true),
                    dmFirstName = table.Column<string>(nullable: true),
                    dmMiddleName = table.Column<string>(nullable: true),
                    dmLastName = table.Column<string>(nullable: true),
                    sxFirstName = table.Column<string>(nullable: true),
                    sxLastName = table.Column<string>(nullable: true),
                    sxMiddleName = table.Column<string>(nullable: true),
                    sxPKValue = table.Column<string>(nullable: true),
                    dmPKValue = table.Column<string>(nullable: true),
                    sxdmPKValue = table.Column<string>(nullable: true),
                    sxPKValueDoB = table.Column<string>(nullable: true),
                    dmPKValueDoB = table.Column<string>(nullable: true),
                    sxdmPKValueDoB = table.Column<string>(nullable: true),
                    FacilityId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientIndices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterSiteScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Score = table.Column<double>(nullable: false),
                    Session = table.Column<Guid>(nullable: false),
                    Rank = table.Column<double>(nullable: false),
                    PatientIndexId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterSiteScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterSiteScores_PatientIndices_PatientIndexId1",
                        column: x => x.PatientIndexId1,
                        principalTable: "PatientIndices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SiteScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Score = table.Column<double>(nullable: false),
                    Session = table.Column<Guid>(nullable: false),
                    Rank = table.Column<double>(nullable: false),
                    PatientIndexId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteScores_PatientIndices_PatientIndexId1",
                        column: x => x.PatientIndexId1,
                        principalTable: "PatientIndices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterSiteScores_PatientIndexId1",
                table: "InterSiteScores",
                column: "PatientIndexId1");

            migrationBuilder.CreateIndex(
                name: "IX_SiteScores_PatientIndexId1",
                table: "SiteScores",
                column: "PatientIndexId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterSiteScores");

            migrationBuilder.DropTable(
                name: "SiteScores");

            migrationBuilder.DropTable(
                name: "PatientIndices");
        }
    }
}

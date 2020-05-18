using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dwapi.Bot.Infrastructure.Migrations
{
    public partial class BotInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MatchStatus = table.Column<int>(nullable: false),
                    MinThreshold = table.Column<double>(nullable: true),
                    MaxThreshold = table.Column<double>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectIndices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MpiId = table.Column<Guid>(nullable: false),
                    PatientPk = table.Column<int>(nullable: false),
                    SiteCode = table.Column<int>(nullable: false),
                    FacilityName = table.Column<string>(nullable: true),
                    Serial = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
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
                    FacilityId = table.Column<Guid>(nullable: true),
                    RowId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectIndices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectIndexScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ScanLevel = table.Column<int>(nullable: false),
                    ScanLevelCode = table.Column<string>(nullable: true),
                    OtherSubjectIndexId = table.Column<Guid>(nullable: false),
                    Field = table.Column<int>(nullable: false),
                    Score = table.Column<double>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    SubjectIndexId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectIndexScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectIndexScores_SubjectIndices_SubjectIndexId",
                        column: x => x.SubjectIndexId,
                        principalTable: "SubjectIndices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectIndexStages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    StatusInfo = table.Column<string>(nullable: true),
                    StatusDate = table.Column<DateTime>(nullable: false),
                    SubjectIndexId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectIndexStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectIndexStages_SubjectIndices_SubjectIndexId",
                        column: x => x.SubjectIndexId,
                        principalTable: "SubjectIndices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectIndexScores_SubjectIndexId",
                table: "SubjectIndexScores",
                column: "SubjectIndexId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectIndexStages_SubjectIndexId",
                table: "SubjectIndexStages",
                column: "SubjectIndexId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchConfigs");

            migrationBuilder.DropTable(
                name: "SubjectIndexScores");

            migrationBuilder.DropTable(
                name: "SubjectIndexStages");

            migrationBuilder.DropTable(
                name: "SubjectIndices");
        }
    }
}

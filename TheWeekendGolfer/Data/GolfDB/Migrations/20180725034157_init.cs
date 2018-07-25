using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheWeekendGolfer.Data.GolfDB.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Holes = table.Column<string>(nullable: true),
                    TeeName = table.Column<string>(nullable: true),
                    Par = table.Column<int>(nullable: false),
                    ScratchRating = table.Column<decimal>(nullable: false),
                    Slope = table.Column<decimal>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Handicap = table.Column<decimal>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GolfRounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    CourseId = table.Column<Guid>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolfRounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GolfRounds_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: true),
                    Value = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    GolfRoundId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scores_GolfRounds_GolfRoundId",
                        column: x => x.GolfRoundId,
                        principalTable: "GolfRounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Scores_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GolfRounds_CourseId",
                table: "GolfRounds",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_GolfRoundId",
                table: "Scores",
                column: "GolfRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_PlayerId",
                table: "Scores",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "GolfRounds");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheWeekendGolfer.Data.GolfDb.Migrations
{
    public partial class addHandicap : Migration
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
                name: "GolfRounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    CourseId = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolfRounds", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
            name: "FK_GolfRounds_Courses_CourseId",
            table: "GolfRounds",
            column: "CourseId",
            principalTable: "Courses",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);





            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(maxLength: 450, nullable: true),
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
            migrationBuilder.AddForeignKey(
name: "FK_Players_AspNetUser",
table: "Players",
column: "UserId",
principalTable: "AspNetUsers",
principalColumn: "Id",
onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateTable(
    name: "Handicaps",
    columns: table => new
    {
        Id = table.Column<Guid>(nullable: false),
        PlayerId = table.Column<Guid>(nullable: false),
        Value = table.Column<decimal>(nullable: false),
        Date = table.Column<DateTime>(nullable: false)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_Handicaps", x => x.Id);
    });

            migrationBuilder.AddForeignKey(
            name: "FK_Handicaps_Players",
            table: "Handicaps",
            column: "PlayerId",
            principalTable: "Players",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: false),
                    Value = table.Column<int>(nullable: false),
                    GolfRoundId = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
            name: "FK_Scores_Players",
            table: "Scores",
            column: "PlayerId",
            principalTable: "Players",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
            name: "FK_Scores_GolfRounds",
            table: "Scores",
            column: "GolfRoundId",
            principalTable: "GolfRounds",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "GolfRounds");

            migrationBuilder.DropTable(
                name: "Handicaps");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Scores");
        }
    }
}

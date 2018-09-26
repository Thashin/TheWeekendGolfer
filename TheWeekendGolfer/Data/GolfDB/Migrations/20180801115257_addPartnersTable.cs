using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheWeekendGolfer.Data.GolfDb.Migrations
{
    public partial class addPartnersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: false),
                    PartnerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
            name: "FK_Partners_Players_PlayerId",
            table: "Partners",
            column: "PlayerId",
            principalTable: "Players",
            principalColumn: "Id",
            onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
            name: "FK_Partners_Players_PartnerId",
            table: "Partners",
            column: "PartnerId",
            principalTable: "Players",
            principalColumn: "Id",
            onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partners");
        }
    }
}

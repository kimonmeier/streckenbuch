using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedHalteorte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HalteortHead",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BetriebspunktId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GueltigVon = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    GueltigBis = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HalteortHead", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HalteortHead_Betriebspunkt_BetriebspunktId",
                        column: x => x.BetriebspunktId,
                        principalTable: "Betriebspunkt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HalteortPositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    HalteortHeadId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Typ = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HalteortPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HalteortPositions_HalteortHead_HalteortHeadId",
                        column: x => x.HalteortHeadId,
                        principalTable: "HalteortHead",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HalteortHead_BetriebspunktId",
                table: "HalteortHead",
                column: "BetriebspunktId");

            migrationBuilder.CreateIndex(
                name: "IX_HalteortPositions_HalteortHeadId",
                table: "HalteortPositions",
                column: "HalteortHeadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HalteortPositions");

            migrationBuilder.DropTable(
                name: "HalteortHead");
        }
    }
}

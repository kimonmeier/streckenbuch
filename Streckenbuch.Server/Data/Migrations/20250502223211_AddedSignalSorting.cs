using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedSignalSorting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SignalStreckenZuordnungSortingStrecke",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GueltigVon = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    GueltigBis = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    StreckenKonfigurationId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignalStreckenZuordnungSortingStrecke", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignalStreckenZuordnungSortingStrecke_StreckenKonfiguration_StreckenKonfigurationId",
                        column: x => x.StreckenKonfigurationId,
                        principalTable: "StreckenKonfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SignalStreckenZuordnungSortingBetriebspunkt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SignalStreckenZuordnungSortingStreckeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BetriebspunktStreckenZuordnungId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignalStreckenZuordnungSortingBetriebspunkt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignalStreckenZuordnungSortingBetriebspunkt_BetriebspunktStreckenZuordnung_BetriebspunktStreckenZuordnungId",
                        column: x => x.BetriebspunktStreckenZuordnungId,
                        principalTable: "BetriebspunktStreckenZuordnung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SignalStreckenZuordnungSortingBetriebspunkt_SignalStreckenZuordnungSortingStrecke_SignalStreckenZuordnungSortingStreckeId",
                        column: x => x.SignalStreckenZuordnungSortingStreckeId,
                        principalTable: "SignalStreckenZuordnungSortingStrecke",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SignalStreckenZuordnungSortingSignal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SignalStreckenZuordnungId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SignalStreckenZuordnungSortingBetriebspunktId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SortingOrder = table.Column<short>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignalStreckenZuordnungSortingSignal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignalStreckenZuordnungSortingSignal_SignalStreckenZuordnungSortingBetriebspunkt_SignalStreckenZuordnungSortingBetriebspunktId",
                        column: x => x.SignalStreckenZuordnungSortingBetriebspunktId,
                        principalTable: "SignalStreckenZuordnungSortingBetriebspunkt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SignalStreckenZuordnungSortingSignal_SignalStreckenZuordnung_SignalStreckenZuordnungId",
                        column: x => x.SignalStreckenZuordnungId,
                        principalTable: "SignalStreckenZuordnung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SignalStreckenZuordnungSortingBetriebspunkt_BetriebspunktStreckenZuordnungId",
                table: "SignalStreckenZuordnungSortingBetriebspunkt",
                column: "BetriebspunktStreckenZuordnungId");

            migrationBuilder.CreateIndex(
                name: "IX_SignalStreckenZuordnungSortingBetriebspunkt_SignalStreckenZuordnungSortingStreckeId",
                table: "SignalStreckenZuordnungSortingBetriebspunkt",
                column: "SignalStreckenZuordnungSortingStreckeId");

            migrationBuilder.CreateIndex(
                name: "IX_SignalStreckenZuordnungSortingSignal_SignalStreckenZuordnungId",
                table: "SignalStreckenZuordnungSortingSignal",
                column: "SignalStreckenZuordnungId");

            migrationBuilder.CreateIndex(
                name: "IX_SignalStreckenZuordnungSortingSignal_SignalStreckenZuordnungSortingBetriebspunktId",
                table: "SignalStreckenZuordnungSortingSignal",
                column: "SignalStreckenZuordnungSortingBetriebspunktId");

            migrationBuilder.CreateIndex(
                name: "IX_SignalStreckenZuordnungSortingStrecke_GueltigVon_GueltigBis",
                table: "SignalStreckenZuordnungSortingStrecke",
                columns: new[] { "GueltigVon", "GueltigBis" });

            migrationBuilder.CreateIndex(
                name: "IX_SignalStreckenZuordnungSortingStrecke_StreckenKonfigurationId",
                table: "SignalStreckenZuordnungSortingStrecke",
                column: "StreckenKonfigurationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignalStreckenZuordnungSortingSignal");

            migrationBuilder.DropTable(
                name: "SignalStreckenZuordnungSortingBetriebspunkt");

            migrationBuilder.DropTable(
                name: "SignalStreckenZuordnungSortingStrecke");
        }
    }
}

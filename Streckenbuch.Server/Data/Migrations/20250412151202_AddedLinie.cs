using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedLinie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Linie",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nummer = table.Column<int>(type: "INTEGER", nullable: false),
                    Typ = table.Column<short>(type: "INTEGER", nullable: false),
                    VonBetriebspunktId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BisBetriebspunktId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Linie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Linie_Betriebspunkt_BisBetriebspunktId",
                        column: x => x.BisBetriebspunktId,
                        principalTable: "Betriebspunkt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Linie_Betriebspunkt_VonBetriebspunktId",
                        column: x => x.VonBetriebspunktId,
                        principalTable: "Betriebspunkt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinienStreckenKonfigurationen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    LinieId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StreckenKonfigurationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    VonBetriebspunktId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BisBetriebspunktId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinienStreckenKonfigurationen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinienStreckenKonfigurationen_Betriebspunkt_BisBetriebspunktId",
                        column: x => x.BisBetriebspunktId,
                        principalTable: "Betriebspunkt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinienStreckenKonfigurationen_Betriebspunkt_VonBetriebspunktId",
                        column: x => x.VonBetriebspunktId,
                        principalTable: "Betriebspunkt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinienStreckenKonfigurationen_Linie_LinieId",
                        column: x => x.LinieId,
                        principalTable: "Linie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinienStreckenKonfigurationen_StreckenKonfiguration_StreckenKonfigurationId",
                        column: x => x.StreckenKonfigurationId,
                        principalTable: "StreckenKonfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Linie_BisBetriebspunktId",
                table: "Linie",
                column: "BisBetriebspunktId");

            migrationBuilder.CreateIndex(
                name: "IX_Linie_Typ_Nummer_VonBetriebspunktId_BisBetriebspunktId",
                table: "Linie",
                columns: new[] { "Typ", "Nummer", "VonBetriebspunktId", "BisBetriebspunktId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Linie_VonBetriebspunktId",
                table: "Linie",
                column: "VonBetriebspunktId");

            migrationBuilder.CreateIndex(
                name: "IX_LinienStreckenKonfigurationen_BisBetriebspunktId",
                table: "LinienStreckenKonfigurationen",
                column: "BisBetriebspunktId");

            migrationBuilder.CreateIndex(
                name: "IX_LinienStreckenKonfigurationen_LinieId_StreckenKonfigurationId",
                table: "LinienStreckenKonfigurationen",
                columns: new[] { "LinieId", "StreckenKonfigurationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LinienStreckenKonfigurationen_StreckenKonfigurationId",
                table: "LinienStreckenKonfigurationen",
                column: "StreckenKonfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_LinienStreckenKonfigurationen_VonBetriebspunktId",
                table: "LinienStreckenKonfigurationen",
                column: "VonBetriebspunktId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinienStreckenKonfigurationen");

            migrationBuilder.DropTable(
                name: "Linie");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BetriebspunktStreckenZuordnung_StreckenKonfigurationId_BetriebspunktId",
                table: "BetriebspunktStreckenZuordnung");

            migrationBuilder.DropIndex(
                name: "IX_BetriebspunktStreckenZuordnung_StreckenKonfigurationId_SortNummer",
                table: "BetriebspunktStreckenZuordnung");

            migrationBuilder.CreateIndex(
                name: "IX_BetriebspunktStreckenZuordnung_StreckenKonfigurationId",
                table: "BetriebspunktStreckenZuordnung",
                column: "StreckenKonfigurationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BetriebspunktStreckenZuordnung_StreckenKonfigurationId",
                table: "BetriebspunktStreckenZuordnung");

            migrationBuilder.CreateIndex(
                name: "IX_BetriebspunktStreckenZuordnung_StreckenKonfigurationId_BetriebspunktId",
                table: "BetriebspunktStreckenZuordnung",
                columns: new[] { "StreckenKonfigurationId", "BetriebspunktId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BetriebspunktStreckenZuordnung_StreckenKonfigurationId_SortNummer",
                table: "BetriebspunktStreckenZuordnung",
                columns: new[] { "StreckenKonfigurationId", "SortNummer" },
                unique: true);
        }
    }
}

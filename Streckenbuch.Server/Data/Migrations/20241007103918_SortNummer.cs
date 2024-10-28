using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class SortNummer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortNummer",
                table: "BetriebspunktStreckenZuordnung",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BetriebspunktStreckenZuordnung_StreckenKonfigurationId_SortNummer",
                table: "BetriebspunktStreckenZuordnung",
                columns: new[] { "StreckenKonfigurationId", "SortNummer" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BetriebspunktStreckenZuordnung_StreckenKonfigurationId_SortNummer",
                table: "BetriebspunktStreckenZuordnung");

            migrationBuilder.DropColumn(
                name: "SortNummer",
                table: "BetriebspunktStreckenZuordnung");
        }
    }
}

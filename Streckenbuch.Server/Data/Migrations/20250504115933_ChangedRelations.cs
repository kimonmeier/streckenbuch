using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignalStreckenZuordnungSortingBetriebspunkt_BetriebspunktStreckenZuordnung_BetriebspunktStreckenZuordnungId",
                table: "SignalStreckenZuordnungSortingBetriebspunkt");

            migrationBuilder.DropForeignKey(
                name: "FK_SignalStreckenZuordnungSortingSignal_SignalStreckenZuordnung_SignalStreckenZuordnungId",
                table: "SignalStreckenZuordnungSortingSignal");

            migrationBuilder.RenameColumn(
                name: "SignalStreckenZuordnungId",
                table: "SignalStreckenZuordnungSortingSignal",
                newName: "SignalId");

            migrationBuilder.RenameIndex(
                name: "IX_SignalStreckenZuordnungSortingSignal_SignalStreckenZuordnungId",
                table: "SignalStreckenZuordnungSortingSignal",
                newName: "IX_SignalStreckenZuordnungSortingSignal_SignalId");

            migrationBuilder.RenameColumn(
                name: "BetriebspunktStreckenZuordnungId",
                table: "SignalStreckenZuordnungSortingBetriebspunkt",
                newName: "BetriebspunktId");

            migrationBuilder.RenameIndex(
                name: "IX_SignalStreckenZuordnungSortingBetriebspunkt_BetriebspunktStreckenZuordnungId",
                table: "SignalStreckenZuordnungSortingBetriebspunkt",
                newName: "IX_SignalStreckenZuordnungSortingBetriebspunkt_BetriebspunktId");

            migrationBuilder.AddForeignKey(
                name: "FK_SignalStreckenZuordnungSortingBetriebspunkt_Betriebspunkt_BetriebspunktId",
                table: "SignalStreckenZuordnungSortingBetriebspunkt",
                column: "BetriebspunktId",
                principalTable: "Betriebspunkt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SignalStreckenZuordnungSortingSignal_Signal_SignalId",
                table: "SignalStreckenZuordnungSortingSignal",
                column: "SignalId",
                principalTable: "Signal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignalStreckenZuordnungSortingBetriebspunkt_Betriebspunkt_BetriebspunktId",
                table: "SignalStreckenZuordnungSortingBetriebspunkt");

            migrationBuilder.DropForeignKey(
                name: "FK_SignalStreckenZuordnungSortingSignal_Signal_SignalId",
                table: "SignalStreckenZuordnungSortingSignal");

            migrationBuilder.RenameColumn(
                name: "SignalId",
                table: "SignalStreckenZuordnungSortingSignal",
                newName: "SignalStreckenZuordnungId");

            migrationBuilder.RenameIndex(
                name: "IX_SignalStreckenZuordnungSortingSignal_SignalId",
                table: "SignalStreckenZuordnungSortingSignal",
                newName: "IX_SignalStreckenZuordnungSortingSignal_SignalStreckenZuordnungId");

            migrationBuilder.RenameColumn(
                name: "BetriebspunktId",
                table: "SignalStreckenZuordnungSortingBetriebspunkt",
                newName: "BetriebspunktStreckenZuordnungId");

            migrationBuilder.RenameIndex(
                name: "IX_SignalStreckenZuordnungSortingBetriebspunkt_BetriebspunktId",
                table: "SignalStreckenZuordnungSortingBetriebspunkt",
                newName: "IX_SignalStreckenZuordnungSortingBetriebspunkt_BetriebspunktStreckenZuordnungId");

            migrationBuilder.AddForeignKey(
                name: "FK_SignalStreckenZuordnungSortingBetriebspunkt_BetriebspunktStreckenZuordnung_BetriebspunktStreckenZuordnungId",
                table: "SignalStreckenZuordnungSortingBetriebspunkt",
                column: "BetriebspunktStreckenZuordnungId",
                principalTable: "BetriebspunktStreckenZuordnung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SignalStreckenZuordnungSortingSignal_SignalStreckenZuordnung_SignalStreckenZuordnungId",
                table: "SignalStreckenZuordnungSortingSignal",
                column: "SignalStreckenZuordnungId",
                principalTable: "SignalStreckenZuordnung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

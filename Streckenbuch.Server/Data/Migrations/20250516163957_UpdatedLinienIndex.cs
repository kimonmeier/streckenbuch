using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedLinienIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LinienStreckenKonfigurationen_LinieId_StreckenKonfigurationId",
                table: "LinienStreckenKonfigurationen");

            migrationBuilder.CreateIndex(
                name: "IX_LinienStreckenKonfigurationen_LinieId_StreckenKonfigurationId_BisBetriebspunktId",
                table: "LinienStreckenKonfigurationen",
                columns: new[] { "LinieId", "StreckenKonfigurationId", "BisBetriebspunktId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LinienStreckenKonfigurationen_LinieId_StreckenKonfigurationId_VonBetriebspunktId",
                table: "LinienStreckenKonfigurationen",
                columns: new[] { "LinieId", "StreckenKonfigurationId", "VonBetriebspunktId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LinienStreckenKonfigurationen_LinieId_StreckenKonfigurationId_BisBetriebspunktId",
                table: "LinienStreckenKonfigurationen");

            migrationBuilder.DropIndex(
                name: "IX_LinienStreckenKonfigurationen_LinieId_StreckenKonfigurationId_VonBetriebspunktId",
                table: "LinienStreckenKonfigurationen");

            migrationBuilder.CreateIndex(
                name: "IX_LinienStreckenKonfigurationen_LinieId_StreckenKonfigurationId",
                table: "LinienStreckenKonfigurationen",
                columns: new[] { "LinieId", "StreckenKonfigurationId" },
                unique: true);
        }
    }
}

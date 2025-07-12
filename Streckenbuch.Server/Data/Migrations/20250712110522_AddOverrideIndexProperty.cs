using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOverrideIndexProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OverrideIndex",
                table: "SignalStreckenZuordnung",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverrideIndex",
                table: "SignalStreckenZuordnung");
        }
    }
}

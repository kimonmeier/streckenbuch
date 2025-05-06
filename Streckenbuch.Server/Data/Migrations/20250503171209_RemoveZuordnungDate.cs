using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveZuordnungDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GueltigAb",
                table: "SignalStreckenZuordnung");

            migrationBuilder.DropColumn(
                name: "GueltigBis",
                table: "SignalStreckenZuordnung");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "GueltigAb",
                table: "SignalStreckenZuordnung",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "GueltigBis",
                table: "SignalStreckenZuordnung",
                type: "TEXT",
                nullable: true);
        }
    }
}

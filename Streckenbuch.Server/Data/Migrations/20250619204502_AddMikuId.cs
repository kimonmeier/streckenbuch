using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMikuId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MikuId",
                table: "Betriebspunkt",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Betriebspunkt_MikuId",
                table: "Betriebspunkt",
                column: "MikuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Betriebspunkt_MikuId",
                table: "Betriebspunkt");

            migrationBuilder.DropColumn(
                name: "MikuId",
                table: "Betriebspunkt");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedRelationOfLinieTrain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LinieTrain_LinieId",
                table: "LinieTrain");

            migrationBuilder.CreateIndex(
                name: "IX_LinieTrain_LinieId",
                table: "LinieTrain",
                column: "LinieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LinieTrain_LinieId",
                table: "LinieTrain");

            migrationBuilder.CreateIndex(
                name: "IX_LinieTrain_LinieId",
                table: "LinieTrain",
                column: "LinieId",
                unique: true);
        }
    }
}

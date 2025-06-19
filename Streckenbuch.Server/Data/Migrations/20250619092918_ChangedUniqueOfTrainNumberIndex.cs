using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUniqueOfTrainNumberIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LinieTrain_TrainNumber",
                table: "LinieTrain");

            migrationBuilder.CreateIndex(
                name: "IX_LinieTrain_TrainNumber",
                table: "LinieTrain",
                column: "TrainNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LinieTrain_TrainNumber",
                table: "LinieTrain");

            migrationBuilder.CreateIndex(
                name: "IX_LinieTrain_TrainNumber",
                table: "LinieTrain",
                column: "TrainNumber");
        }
    }
}

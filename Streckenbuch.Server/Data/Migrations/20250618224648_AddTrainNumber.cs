using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LinieTrain",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    LinieId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TrainNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinieTrain", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinieTrain_Linie_LinieId",
                        column: x => x.LinieId,
                        principalTable: "Linie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinieTrain_LinieId",
                table: "LinieTrain",
                column: "LinieId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LinieTrain_TrainNumber",
                table: "LinieTrain",
                column: "TrainNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinieTrain");
        }
    }
}

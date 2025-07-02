using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedRecording : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkDriver",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TrainDriverNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDriver", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkShift",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkDriverId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Datum = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkShift", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkShift_WorkDriver_WorkDriverId",
                        column: x => x.WorkDriverId,
                        principalTable: "WorkDriver",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkTrip",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkShiftId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TripNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTrip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTrip_WorkShift_WorkShiftId",
                        column: x => x.WorkShiftId,
                        principalTable: "WorkShift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripRecording",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkTripId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Location_X = table.Column<double>(type: "REAL", nullable: false),
                    Location_Y = table.Column<double>(type: "REAL", nullable: false),
                    Time = table.Column<TimeOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripRecording", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripRecording_WorkTrip_WorkTripId",
                        column: x => x.WorkTripId,
                        principalTable: "WorkTrip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TripRecording_WorkTripId",
                table: "TripRecording",
                column: "WorkTripId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDriver_TrainDriverNumber",
                table: "WorkDriver",
                column: "TrainDriverNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkShift_WorkDriverId",
                table: "WorkShift",
                column: "WorkDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTrip_WorkShiftId",
                table: "WorkTrip",
                column: "WorkShiftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TripRecording");

            migrationBuilder.DropTable(
                name: "WorkTrip");

            migrationBuilder.DropTable(
                name: "WorkShift");

            migrationBuilder.DropTable(
                name: "WorkDriver");
        }
    }
}

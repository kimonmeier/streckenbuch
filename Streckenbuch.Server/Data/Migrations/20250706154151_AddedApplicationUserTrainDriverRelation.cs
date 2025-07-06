using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedApplicationUserTrainDriverRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "WorkDriver",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkDriver_ApplicationUserId",
                table: "WorkDriver",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkDriver_AspNetUsers_ApplicationUserId",
                table: "WorkDriver",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkDriver_AspNetUsers_ApplicationUserId",
                table: "WorkDriver");

            migrationBuilder.DropIndex(
                name: "IX_WorkDriver_ApplicationUserId",
                table: "WorkDriver");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "WorkDriver");
        }
    }
}

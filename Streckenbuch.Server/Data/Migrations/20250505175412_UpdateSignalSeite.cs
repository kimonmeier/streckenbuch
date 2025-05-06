using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streckenbuch.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSignalSeite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Signal SET Seite = 0 WHERE Seite = 1");
            migrationBuilder.Sql("UPDATE Signal SET Seite = 1 WHERE Seite = 2");
            migrationBuilder.Sql("UPDATE Signal SET Seite = 1 WHERE Seite = 3");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

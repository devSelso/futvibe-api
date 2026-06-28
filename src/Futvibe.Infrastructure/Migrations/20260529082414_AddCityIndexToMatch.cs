using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Futvibe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCityIndexToMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_matches_city",
                table: "matches",
                column: "city");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_matches_city",
                table: "matches");
        }
    }
}

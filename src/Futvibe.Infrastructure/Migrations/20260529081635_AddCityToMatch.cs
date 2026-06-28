using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Futvibe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCityToMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "matches",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "city",
                table: "matches");
        }
    }
}

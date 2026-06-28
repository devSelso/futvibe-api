using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Futvibe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchesAttended : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchesAttended",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchesAttended",
                table: "users");
        }
    }
}

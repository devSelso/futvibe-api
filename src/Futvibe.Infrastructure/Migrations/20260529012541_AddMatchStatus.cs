using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Futvibe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "matches",
                type: "text",
                nullable: false,
                defaultValue: "Scheduled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "matches");
        }
    }
}

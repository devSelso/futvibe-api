using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Futvibe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "banners",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    alt = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    href = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    display_order = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banners", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "matches",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    location = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    level = table.Column<string>(type: "text", nullable: false),
                    price_per_player = table.Column<decimal>(type: "numeric(10,2)", nullable: false, defaultValue: 0m),
                    max_players = table.Column<int>(type: "integer", nullable: false),
                    visibility = table.Column<string>(type: "text", nullable: false),
                    host_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_matches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    avatar = table.Column<string>(type: "text", nullable: true),
                    bio = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    level = table.Column<string>(type: "text", nullable: false),
                    presence_score = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    matches_played = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "participants",
                columns: table => new
                {
                    match_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_participants", x => new { x.match_id, x.user_id });
                    table.ForeignKey(
                        name: "FK_participants_matches_match_id",
                        column: x => x.match_id,
                        principalTable: "matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_participants_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_matches_date",
                table: "matches",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "IX_matches_host_id",
                table: "matches",
                column: "host_id");

            migrationBuilder.CreateIndex(
                name: "IX_matches_level",
                table: "matches",
                column: "level");

            migrationBuilder.CreateIndex(
                name: "IX_participants_user_id",
                table: "participants",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "banners");

            migrationBuilder.DropTable(
                name: "participants");

            migrationBuilder.DropTable(
                name: "matches");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

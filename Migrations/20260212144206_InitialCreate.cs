using System;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GestorHeroesRPG.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "game");

            migrationBuilder.CreateTable(
                name: "character",
                schema: "game",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    level = table.Column<int>(type: "integer", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    guild = table.Column<string>(type: "text", nullable: true),
                    traits = table.Column<JsonNode>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "archer",
                schema: "game",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    precision = table.Column<double>(type: "double precision", nullable: false),
                    has_pet = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_archer", x => x.id);
                    table.ForeignKey(
                        name: "FK_archer_character_id",
                        column: x => x.id,
                        principalSchema: "game",
                        principalTable: "character",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cleric",
                schema: "game",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    deity = table.Column<string>(type: "text", nullable: false),
                    healing_points = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cleric", x => x.id);
                    table.ForeignKey(
                        name: "FK_cleric_character_id",
                        column: x => x.id,
                        principalSchema: "game",
                        principalTable: "character",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mage",
                schema: "game",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    mana = table.Column<int>(type: "integer", nullable: false),
                    main_element = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mage", x => x.id);
                    table.ForeignKey(
                        name: "FK_mage_character_id",
                        column: x => x.id,
                        principalSchema: "game",
                        principalTable: "character",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "warrior",
                schema: "game",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    main_weapon = table.Column<string>(type: "text", nullable: false),
                    fury = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warrior", x => x.id);
                    table.ForeignKey(
                        name: "FK_warrior_character_id",
                        column: x => x.id,
                        principalSchema: "game",
                        principalTable: "character",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_character_name",
                schema: "game",
                table: "character",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "archer",
                schema: "game");

            migrationBuilder.DropTable(
                name: "cleric",
                schema: "game");

            migrationBuilder.DropTable(
                name: "mage",
                schema: "game");

            migrationBuilder.DropTable(
                name: "warrior",
                schema: "game");

            migrationBuilder.DropTable(
                name: "character",
                schema: "game");
        }
    }
}

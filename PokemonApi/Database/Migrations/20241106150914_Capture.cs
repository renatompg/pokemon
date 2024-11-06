using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class Capture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Captures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PokemonMasterId = table.Column<int>(type: "INTEGER", nullable: false),
                    PokemonName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Captures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Captures_PokemonMasters_PokemonMasterId",
                        column: x => x.PokemonMasterId,
                        principalTable: "PokemonMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Captures_PokemonMasterId_PokemonName",
                table: "Captures",
                columns: new[] { "PokemonMasterId", "PokemonName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Captures");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Importador.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class MigrationInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Catalago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalago", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalagoColuna",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCatalago = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalagoColuna", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalagoColuna_Catalago_IdCatalago",
                        column: x => x.IdCatalago,
                        principalTable: "Catalago",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DadosCatalago",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCatalagoColuna = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Linhas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosCatalago", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DadosCatalago_CatalagoColuna_IdCatalagoColuna",
                        column: x => x.IdCatalagoColuna,
                        principalTable: "CatalagoColuna",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalagoColuna_IdCatalago",
                table: "CatalagoColuna",
                column: "IdCatalago");

            migrationBuilder.CreateIndex(
                name: "IX_DadosCatalago_IdCatalagoColuna",
                table: "DadosCatalago",
                column: "IdCatalagoColuna");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DadosCatalago");

            migrationBuilder.DropTable(
                name: "CatalagoColuna");

            migrationBuilder.DropTable(
                name: "Catalago");
        }
    }
}

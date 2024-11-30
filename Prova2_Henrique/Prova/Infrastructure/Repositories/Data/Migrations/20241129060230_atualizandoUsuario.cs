using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prova.Migrations
{
    /// <inheritdoc />
    public partial class atualizandoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListaDeProdutosPorClientes");

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Usuarios");

            migrationBuilder.CreateTable(
                name: "ListaDeProdutosPorClientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaDeProdutosPorClientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListaDeProdutosPorClientes_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListaDeProdutosPorClientes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListaDeProdutosPorClientes_ProdutoId",
                table: "ListaDeProdutosPorClientes",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ListaDeProdutosPorClientes_UsuarioId",
                table: "ListaDeProdutosPorClientes",
                column: "UsuarioId");
        }
    }
}

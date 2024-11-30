using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prova.Migrations
{
    /// <inheritdoc />
    public partial class mudandoOTipoDaIamgemNovamente5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemUrl",
                table: "Usuarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagemUrl",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

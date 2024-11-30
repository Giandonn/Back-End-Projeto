using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prova.Migrations
{
    /// <inheritdoc />
    public partial class mudandoOTipoDaIamgemNovamente7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagemBase64",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemBase64",
                table: "Usuarios");
        }
    }
}

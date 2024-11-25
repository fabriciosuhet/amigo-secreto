using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presentes.Migrations
{
    /// <inheritdoc />
    public partial class AddAtivoAmigoSecreto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "AmigosSecretos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "AmigosSecretos");
        }
    }
}

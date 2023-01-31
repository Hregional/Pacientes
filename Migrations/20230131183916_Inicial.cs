using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatosPacientes.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grupo_Sanguineo",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "int", nullable: false, comment: "Almacena el Codigo de Grupo Sanguineo")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, comment: "Almacena el Nombre de Grupo Sanguineo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupo_Sanguineo", x => x.Codigo);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Grupo_Sanguineo");
        }
    }
}

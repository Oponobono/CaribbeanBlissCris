using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Caribbean2.Migrations
{
    /// <inheritdoc />
    public partial class AddIdRoltoUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdRol",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RolIdRol",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolIdRol",
                table: "Usuarios",
                column: "RolIdRol");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Roles_RolIdRol",
                table: "Usuarios",
                column: "RolIdRol",
                principalTable: "Roles",
                principalColumn: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Roles_RolIdRol",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_RolIdRol",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdRol",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "RolIdRol",
                table: "Usuarios");
        }
    }
}

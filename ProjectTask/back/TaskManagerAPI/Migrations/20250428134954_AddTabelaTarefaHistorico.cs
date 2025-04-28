using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTabelaTarefaHistorico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TarefasHistoricos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TarefaId = table.Column<int>(type: "INTEGER", nullable: false),
                    CampoAlterado = table.Column<string>(type: "TEXT", nullable: false),
                    ValorAnterior = table.Column<string>(type: "TEXT", nullable: false),
                    ValorNovo = table.Column<string>(type: "TEXT", nullable: false),
                    DataModificacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UsuarioModificador = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TarefasHistoricos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TarefasHistoricos_Tasks_TarefaId",
                        column: x => x.TarefaId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TarefasHistoricos_TarefaId",
                table: "TarefasHistoricos",
                column: "TarefaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TarefasHistoricos");
        }
    }
}

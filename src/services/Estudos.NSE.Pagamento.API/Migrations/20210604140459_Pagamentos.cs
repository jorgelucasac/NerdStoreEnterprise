using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Estudos.NSE.Pagamentos.API.Migrations
{
    public partial class Pagamentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pagamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PedidoId = table.Column<Guid>(nullable: false),
                    TipoPagamento = table.Column<int>(nullable: false),
                    Valor = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CodigoAutorizacao = table.Column<string>(type: "varchar(100)", nullable: true),
                    BandeiraCartao = table.Column<string>(type: "varchar(100)", nullable: true),
                    DataTransacao = table.Column<DateTime>(nullable: true),
                    ValorTotal = table.Column<decimal>(nullable: false),
                    CustoTransacao = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TID = table.Column<string>(type: "varchar(100)", nullable: true),
                    NSU = table.Column<string>(type: "varchar(100)", nullable: true),
                    PagamentoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacao_Pagamento_PagamentoId",
                        column: x => x.PagamentoId,
                        principalTable: "Pagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_PagamentoId",
                table: "Transacao",
                column: "PagamentoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transacao");

            migrationBuilder.DropTable(
                name: "Pagamento");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Estudos.NSE.Pedidos.API.Application.DTO;
using Estudos.NSE.Pedidos.Domain.Pedidos;

namespace Estudos.NSE.Pedidos.API.Application.Queries
{
    public interface IPedidoQueries
    {
        Task<PedidoDto> ObterUltimoPedido(Guid clienteId);
        Task<IEnumerable<PedidoDto>> ObterListaPorClienteId(Guid clienteId);
        Task<PedidoDto> ObterPedidosAutorizados();
    }

    public class PedidoQueries : IPedidoQueries
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoQueries(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }
        public async Task<PedidoDto> ObterUltimoPedido(Guid clienteId)
        {
            const string sql = @"SELECT
                                P.ID AS 'ProdutoId', P.CODIGO, P.VOUCHERUTILIZADO, P.DESCONTO, P.VALORTOTAL,P.PEDIDOSTATUS,
                                P.LOGRADOURO,P.NUMERO, P.BAIRRO, P.CEP, P.COMPLEMENTO, P.CIDADE, P.ESTADO,
                                PIT.ID AS 'ProdutoItemId',PIT.PRODUTONOME, PIT.QUANTIDADE, PIT.PRODUTOIMAGEM, PIT.VALORUNITARIO 
                                FROM PEDIDO P 
                                INNER JOIN PEDIDOITEM PIT ON P.ID = PIT.PEDIDOID 
                                WHERE P.CLIENTEID = @clienteId 
                                AND P.DATACADASTRO between DATEADD(minute, -3,  GETDATE()) and DATEADD(minute, 0,  GETDATE())
                                AND P.PEDIDOSTATUS = 1 
                                ORDER BY P.DATACADASTRO DESC";

            var pedido = await _pedidoRepository.ObterConexao().QueryAsync<dynamic>(sql, new { clienteId });

            return MapearPedido(pedido);
        }

        public async Task<IEnumerable<PedidoDto>> ObterListaPorClienteId(Guid clienteId)
        {
            var pedidos = await _pedidoRepository.ObterListaPorClienteId(clienteId);

            return pedidos.Select(PedidoDto.ParaPedidoDto);
        }

        public async Task<PedidoDto> ObterPedidosAutorizados()
        {
            // Correção para pegar todos os itens do pedido e ordernar pelo pedido mais antigo
            const string sql = @"SELECT 
                                P.ID as 'PedidoId', P.ID, P.CLIENTEID, 
                                PI.ID as 'PedidoItemId', PI.ID, PI.PRODUTOID, PI.QUANTIDADE 
                                FROM PEDIDO P 
                                INNER JOIN PEDIDOITEM PI ON P.ID = PI.PEDIDOID 
                                WHERE P.PEDIDOSTATUS = 1                                
                                ORDER BY P.DATACADASTRO";

            // Utilizacao do lookup para manter o estado a cada ciclo de registro retornado
            var lookup = new Dictionary<Guid, PedidoDto>();

            await _pedidoRepository.ObterConexao().QueryAsync<PedidoDto, PedidoItemDto, PedidoDto>(sql,
                (p, pi) =>
                {
                    if (!lookup.TryGetValue(p.Id, out var pedidoDto))
                        lookup.Add(p.Id, pedidoDto = p);

                    pedidoDto.PedidoItems ??= new List<PedidoItemDto>();
                    pedidoDto.PedidoItems.Add(pi);

                    return pedidoDto;

                }, splitOn: "PedidoId,PedidoItemId");

            // Obtendo dados o lookup
            return lookup.Values.OrderBy(p => p.Data).FirstOrDefault();
        }

        private static PedidoDto MapearPedido(dynamic result)
        {
            var pedido = new PedidoDto
            {
                Codigo = result[0].CODIGO,
                Status = result[0].PEDIDOSTATUS,
                ValorTotal = result[0].VALORTOTAL,
                Desconto = result[0].DESCONTO,
                VoucherUtilizado = result[0].VOUCHERUTILIZADO,

                PedidoItems = new List<PedidoItemDto>(),
                Endereco = new EnderecoDto
                {
                    Logradouro = result[0].LOGRADOURO,
                    Bairro = result[0].BAIRRO,
                    Cep = result[0].CEP,
                    Cidade = result[0].CIDADE,
                    Complemento = result[0].COMPLEMENTO,
                    Estado = result[0].ESTADO,
                    Numero = result[0].NUMERO
                }
            };

            foreach (var item in result)
            {
                var pedidoItem = new PedidoItemDto
                {
                    Nome = item.PRODUTONOME,
                    Valor = item.VALORUNITARIO,
                    Quantidade = item.QUANTIDADE,
                    Imagem = item.PRODUTOIMAGEM
                };

                pedido.PedidoItems.Add(pedidoItem);
            }

            return pedido;
        }
    }
}
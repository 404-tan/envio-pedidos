using backend.application.DTOs.requests;
using backend.application.DTOs.responses;
using backend.application.services.contracts;
using backend.domain;
using backend.infra.repos.contracts;
using backend.infra.security.contracts;

namespace backend.application.services.impl
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly IProdutoService _produtoService;

        public PedidoService(IPedidoRepository pedidoRepository, IUsuarioService usuarioService, IProdutoService produtoService)
        {
            _pedidoRepository = pedidoRepository;
            _usuarioService = usuarioService;
            _produtoService = produtoService;
        }
        public async Task<PedidoResponse> CriarPedido(CriarPedidoRequest request)
        {
            var idCliente = _usuarioService.ObterIdUsuarioAutenticado();
            var produtosDoPedido = await _produtoService.ObterProdutosPorIds(request.Itens.Select(i => i.IdProduto).ToArray());
            var listaItens = new List<(Guid IdProduto, int Quantidade, decimal Preco)>();

            foreach (var item in request.Itens)
            {
                var produto = produtosDoPedido.FirstOrDefault(p => p.Id == item.IdProduto);

                if (produto == null) throw new Exception($"O produto {item.IdProduto} não existe.");
                
                listaItens.Add((item.IdProduto, item.Quantidade, produto.PrecoAtual));
            }

            var pedido = Pedido.Criar(
                idCliente,
                listaItens
            );
            var idPedidoCriado = await _pedidoRepository.CriarPedidoAsync(pedido);
            var pedidoCriado = await _pedidoRepository.ObterPedidoComItensEProdutosPorIdAsync(idPedidoCriado);
            if (pedidoCriado == null) throw new Exception("Erro ao criar o pedido.");
            return new PedidoResponse(
                pedidoCriado.Id,
                pedidoCriado.IdCliente,
                pedidoCriado.Itens.Select(i => new ItemPedidoResponse(
                    i.Id,
                    i.Produto!.Nome,
                    i.PrecoUnitario,
                    i.Quantidade
                )).ToList(),
                pedidoCriado.Total,
                pedidoCriado.StatusAtual,
                pedidoCriado.DataCriacao,
                pedidoCriado.DataAtualizacao
            );
        }

        public async Task<IList<PedidoResponse>> ObterPedidosPorCursorEUsuarioAutenticadoAsync(ListarPedidosRequest request)
        {
            var idCliente = _usuarioService.ObterIdUsuarioAutenticado();
            var isAdmin = await _usuarioService.IsAdminAsync(idCliente);
            var pedidos = await _pedidoRepository.ObterPedidosPorCursorAsync(
                request.Cursor,
                idCliente,
                isAdmin ? null : idCliente,
                request.Limite
            );
            return [.. pedidos.Select(p => new PedidoResponse(
                p.Id,
                p.IdCliente,
                p.Itens.Select(i => new ItemPedidoResponse(
                    i.Id,
                    i.Produto!.Nome,
                    i.PrecoUnitario,
                    i.Quantidade
                )).ToList(),
                p.Total,
                p.StatusAtual,
                p.DataCriacao,
                p.DataAtualizacao
            ))];
        }

        public async Task<ProcessarPedidoResponse> ProcessarPedido(ProcessarPedidoRequest request)
        {
            var usuarioId = _usuarioService.ObterIdUsuarioAutenticado();
            var isAdmin = await _usuarioService.IsAdminAsync(usuarioId);
            if (!isAdmin)
                throw new Exception("Usuário não autorizado a processar pedidos.");
            var nomeAdministrador = await _usuarioService.ObterNomeCompletoUsuarioPorIdAsync(usuarioId);
            var pedido = await _pedidoRepository.ObterPedidoComItensEProdutosPorIdAsync(request.IdPedido);

            if (pedido == null)
                throw new Exception("Pedido não encontrado.");

            pedido.Processar(usuarioId);
            var atualizacaoRealizada = await _pedidoRepository.AtualizarStatusPedidoAsync(pedido);

            if (!atualizacaoRealizada)
                throw new Exception("Erro ao atualizar o pedido.");

            return new ProcessarPedidoResponse(
                pedido.Id,
                pedido.IdCliente,
                nomeAdministrador,
                pedido.Itens.Select(i => new ItemPedidoResponse(
                    i.Id,
                    i.Produto!.Nome,
                    i.PrecoUnitario,
                    i.Quantidade
                )).ToList(),
                pedido.Total,
                pedido.StatusAtual,
                pedido.DataCriacao,
                pedido.DataAtualizacao!.Value
            );
        }
    }
}
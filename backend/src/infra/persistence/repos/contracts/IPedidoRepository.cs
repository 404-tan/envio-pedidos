using backend.domain;
using backend.domain.enums;

namespace backend.infra.repos.contracts
{
    public interface IPedidoRepository
    {
        Task<Guid> CriarPedidoAsync(Pedido pedido);
        Task<IList<Pedido>> ObterPedidosPorCursorEStatusAsync( DateTime cursor,Guid? ultimoId,Guid? usuarioId,PedidoStatus? status, int limite = 10);
        Task<bool> AtualizarStatusPedidoAsync(Pedido pedido);
        Task<Pedido?> ObterPedidoComItensEProdutosPorIdAsync(Guid idPedido);
    }
}
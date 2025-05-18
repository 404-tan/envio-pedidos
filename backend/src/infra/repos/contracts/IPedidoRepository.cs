using backend.domain;

namespace backend.infra.repos.contracts
{
    public interface IPedidoRepository
    {
        Task<Pedido> CriarPedidoAsync(Pedido pedido);
        Task<IList<Pedido>> ObterPedidosPorCursorAsync( DateTime cursor,Guid? ultimoId,Guid? usuarioId, int limite = 10);
        Task<bool> AtualizarStatusPedidoAsync(Pedido pedido);
        Task<Pedido?> ObterPedidoPorIdAsync(Guid idPedido);
    }
}
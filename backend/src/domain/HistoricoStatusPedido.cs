
using backend.domain.enums;

namespace backend.domain
{
    public sealed class HistoricoStatusPedido(Guid pedidoId, PedidoStatus status)
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid PedidoId { get; private set; } = pedidoId;
        public DateTime DataAtualizacao { get; private set; } = DateTime.UtcNow;
        public PedidoStatus Status { get; private set; } = status;
        public Usuario? Administrador { get; private set; }
        public Pedido? Pedido { get; private set; }
    }
}
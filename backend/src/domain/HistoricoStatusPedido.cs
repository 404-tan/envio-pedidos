
using backend.domain.enums;

namespace backend.domain
{
    public sealed class HistoricoStatusPedido
    {
        public Guid Id { get; private set; }
        public Guid PedidoId { get; private set; }
        public DateTime DataAtualizacao { get; private set; }
        public PedidoStatus Status { get; private set; }
        public Guid AdministradorId { get; private set; }
        public Usuario? Administrador { get; private set; }
        private HistoricoStatusPedido() { }
        private HistoricoStatusPedido(Guid pedidoId, Guid adminId, DateTime dataAtualizacao, PedidoStatus status)
        {
            Id = Guid.NewGuid();
            PedidoId = pedidoId;
            AdministradorId = adminId;
            DataAtualizacao = dataAtualizacao;
            Status = status;
        }
        public static HistoricoStatusPedido Criar(Guid pedidoId, Guid adminId, DateTime dataAtualizacao, PedidoStatus status)
        {
            return new HistoricoStatusPedido(pedidoId, adminId, dataAtualizacao, status);
        }

    }
}
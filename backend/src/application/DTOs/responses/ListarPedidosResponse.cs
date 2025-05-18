using backend.domain.enums;

namespace backend.application.DTOs.responses
{
    public sealed record ListarPedidosResponse(
        int Pagina,
        DateTime UltimoCursor,
        List<PedidoResponse> Pedidos
    );
    public sealed record PedidoResponse(
        Guid Id,
        Guid IdCliente,
        List<ItemPedidoResponse> Itens,
        decimal Total,
        PedidoStatus StatusAtual,
        DateTime DataCriacao, 
        DateTime? DataAtualizacao
    );
}

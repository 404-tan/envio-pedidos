
using backend.domain.enums;

namespace backend.application.DTOs.requests
{
    public sealed record ListarPedidosRequest(
        PedidoStatus? Status,
        DateTime? Cursor,
        int Limite = 10
    );
}
using System.ComponentModel.DataAnnotations;
using backend.domain.enums;

namespace backend.application.DTOs.requests
{
    public sealed record ListarPedidosRequest(
        PedidoStatus? Status,

        DateTime? Cursor,

        [Range(1, 100, ErrorMessage = "O limite deve estar entre 1 e 100.")]
        int Limite = 10
    );
}
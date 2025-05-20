using System.ComponentModel.DataAnnotations;

namespace backend.application.DTOs.requests
{
    public sealed record ProcessarPedidoRequest(
        [Required(ErrorMessage = "O Id do pedido é obrigatório.")]
        Guid IdPedido
    );
}
using System.ComponentModel.DataAnnotations;

namespace backend.application.DTOs.requests
{
    public sealed record CriarPedidoRequest(
        [Required(ErrorMessage = "A lista de itens é obrigatória.")]
        [MinLength(1, ErrorMessage = "O pedido deve conter pelo menos um item.")]
        List<ItemPedidoRequest> Itens
    );

    public sealed record ItemPedidoRequest(
        [Required(ErrorMessage = "O Id do produto é obrigatório.")]
        Guid IdProduto,

        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser no mínimo 1.")]
        int Quantidade
    );
}
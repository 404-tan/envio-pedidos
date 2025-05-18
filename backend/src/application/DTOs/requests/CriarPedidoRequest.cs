namespace backend.application.DTOs.requests
{
    public sealed record CriarPedidoRequest(
        Guid IdUsuario,
        List<ItemPedidoRequest> Itens
    );

    public sealed record ItemPedidoRequest(
        Guid IdProduto,
        int Quantidade
    );
}
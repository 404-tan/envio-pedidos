namespace backend.application.DTOs.responses
{
    public sealed record ItemPedidoResponse(
        Guid Id,
        string NomeProduto,
        decimal PrecoUnitario,
        int Quantidade
    );
}
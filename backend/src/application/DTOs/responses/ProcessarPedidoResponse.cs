using backend.domain.enums;

namespace backend.application.DTOs.responses
{
    public sealed record ProcessarPedidoResponse(
        Guid Id,
        Guid IdCliente,
        string NomeAdministradorProcessador,
        List<ItemPedidoResponse> Itens,
        decimal Total,
        PedidoStatus StatusAtual,
        DateTime DataCriacao, 
        DateTime DataAtualizacao
    );
}
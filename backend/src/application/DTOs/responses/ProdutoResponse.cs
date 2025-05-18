namespace backend.application.DTOs.responses
{
    public sealed record ProdutoResponse(
        Guid Id,
        string Nome,
        decimal PrecoAtual
    );
}
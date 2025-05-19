using backend.application.services.contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;

public static class ProdutoEndpoints
{
    public static void MapProdutoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/produtos",[Authorize] async (IProdutoService produtoService) =>
        {
            var produtos = await produtoService.ObterProdutosAsync();
            return Results.Ok(produtos);
        }).WithDescription("Lista todos os produtos disponíveis no sistema. Não requer autenticação.");
        ;
    }
}

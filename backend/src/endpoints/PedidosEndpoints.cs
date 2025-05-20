using backend.application.DTOs.requests;
using backend.application.exceptions;
using backend.application.services.contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;

public static class PedidoEndpoints
{
    public static void MapPedidoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/pedidos", [Authorize] async (CriarPedidoRequest req, IPedidoService service) =>
        {
            try
            {
                var pedido = await service.CriarPedidoAsync(req);
                return Results.Ok(pedido);
            }
            catch (ProdutoNoPedidoInexistenteException ex)
            {
                return Results.BadRequest(new { erro = ex.Message });
            }
            catch (FalhaAoCriarPedidoException ex)
            {
                return Results.Problem(ex.Message, statusCode: 500);
            }
        })
        .WithDescription("Cria um novo pedido pertencente ao usuário autenticado.");

        app.MapPost("/api/pedidos/listar", [Authorize] async (ListarPedidosRequest req, IPedidoService service) =>
        {
            var pedidos = await service.ObterPedidosPorCursorEUsuarioAutenticadoAsync(req);
            return Results.Ok(pedidos);
        })
        .WithDescription("Lista pedidos do usuário autenticado. Se o usuário for cliente, retorna apenas seus pedidos. Se for admin, retorna todos os pedidos.");

        app.MapPut("/api/pedidos/processar", [Authorize(Roles = "Admin")] async (ProcessarPedidoRequest req, IPedidoService service) =>
        {
            try
            {
                var pedidoProcessado = await service.ProcessarPedidoAsync(req);
                return Results.Ok(pedidoProcessado);
            }
            catch (UsuarioNaoAutorizadoException)
            {
                return Results.Forbid();
            }
            catch (PedidoNaoEncontradoException ex)
            {
                return Results.NotFound(new { erro = ex.Message });
            }
            catch (FalhaAtualizarPedidoException ex)
            {
                return Results.Problem(ex.Message, statusCode: 500);
            }
        })
        .WithDescription("Processa um pedido (altera o status para processado). Apenas administradores podem processar pedidos.");
    }
}
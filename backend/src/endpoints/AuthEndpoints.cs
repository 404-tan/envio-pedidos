using backend.application.DTOs.requests;
using backend.application.exceptions;
using backend.application.services.contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/login", async (LoginRequest req, IUsuarioService usuarioService) =>
        {
            try
            {
                var usuario = await usuarioService.LogarUsuarioAsync(req);
                return Results.Ok(usuario);
            }
            catch (UsuarioOuSenhaInvalidosException)
            {
                return Results.Unauthorized();
            }
        });

        app.MapPost("/api/registrar", async (RegistroRequest req, IUsuarioService usuarioService) =>
        {
            try
            {
                var usuario = await usuarioService.RegistrarUsuarioAsync(req);
                return Results.Ok(usuario);
            }
            catch (UsuarioJaExisteException ex)
            {
                return Results.BadRequest(new { erro = ex.Message });
            }
            catch (FalhaAoCriarUsuarioException ex)
            {
                return Results.Problem(ex.Message, statusCode: 500);
            }
        });

        app.MapGet("/api/perfil", [Authorize] async (IUsuarioService usuarioService) =>
        {
            try
            {
                var id = usuarioService.ObterIdUsuarioAutenticado();
                var nome = await usuarioService.ObterNomeCompletoUsuarioPorIdAsync(id);
                var isAdmin = await usuarioService.IsAdminAsync(id);
                return Results.Ok(new { id, nome,isAdmin });
            }
            catch (UsuarioNaoAutenticadoException )
            {
                return Results.Unauthorized();
            }
            catch (UsuarioNaoExisteException ex)
            {
                return Results.NotFound(new { erro = ex.Message });
            }
        });
    }
}
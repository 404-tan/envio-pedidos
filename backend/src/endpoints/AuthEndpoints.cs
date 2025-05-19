using backend.application.DTOs.requests;
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
            var usuario = await usuarioService.LogarUsuarioAsync(req);
            return usuario == null ? Results.Unauthorized() : Results.Ok(usuario);
        });

        app.MapPost("/api/registrar", async (RegistroRequest req, IUsuarioService usuarioService) =>
        {
            var usuario = await usuarioService.RegistrarUsuarioAsync(req);
            return usuario == null ? Results.BadRequest("Erro ao registrar") : Results.Ok(usuario);
        });

        app.MapGet("/api/perfil", [Authorize] async (IUsuarioService usuarioService) =>
        {
            var id = usuarioService.ObterIdUsuarioAutenticado();
            var nome = await usuarioService.ObterNomeCompletoUsuarioPorIdAsync(id);
            return Results.Ok(new { id, nome });
        });
    }
} 
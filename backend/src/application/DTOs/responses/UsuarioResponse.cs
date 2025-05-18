namespace backend.application.DTOs.responses
{
    public sealed record UsuarioResponse(
        Guid Id,
        string NomeCompleto,
        string Email,
        string Token
    );
}
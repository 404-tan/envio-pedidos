namespace backend.application.DTOs.requests
{
    public sealed record RegistroRequest(
        string NomeCompleto,
        string Email,
        string SenhaDigitada
    );
}
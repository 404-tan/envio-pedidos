namespace backend.application.DTOs.requests
{
    public sealed record LoginRequest(
        string Email,
        string SenhaDigitada
    );
}
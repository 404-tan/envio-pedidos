using System.ComponentModel.DataAnnotations;

namespace backend.application.DTOs.requests
{
    public sealed record LoginRequest(
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
        string Email,

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve conter pelo menos 6 caracteres.")]
        string SenhaDigitada
    );
}
using System.ComponentModel.DataAnnotations;

namespace backend.application.DTOs.requests
{
    public sealed record RegistroRequest(
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        string NomeCompleto,

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
        string Email,

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve conter pelo menos 6 caracteres.")]
        string SenhaDigitada
    );
}
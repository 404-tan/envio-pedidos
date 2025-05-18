using backend.application.DTOs.requests;
using backend.application.DTOs.responses;

namespace backend.application.services.contracts
{
    public interface IUsuarioService
    {
        Task<UsuarioResponse> RegistrarUsuarioAsync(RegistroRequest request);
        Task<string> ObterNomeCompletoUsuarioPorIdAsync(Guid idUsuario);
        Task<UsuarioResponse> LogarUsuarioAsync(LoginRequest request);
        Task<bool> IsAdminAsync(Guid idUsuario);
        Guid ObterIdUsuarioAutenticado();
    }
}
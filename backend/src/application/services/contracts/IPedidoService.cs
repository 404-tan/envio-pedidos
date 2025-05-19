using backend.application.DTOs.requests;
using backend.application.DTOs.responses;

namespace backend.application.services.contracts
{
    public interface IPedidoService
    {
        Task<PedidoResponse> CriarPedidoAsync(CriarPedidoRequest request);
        Task<IList<PedidoResponse>> ObterPedidosPorCursorEUsuarioAutenticadoAsync(ListarPedidosRequest request);
        Task<ProcessarPedidoResponse> ProcessarPedidoAsync(ProcessarPedidoRequest request);
    }
}
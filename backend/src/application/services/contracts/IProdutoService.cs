using backend.application.DTOs.responses;
namespace backend.application.services.contracts;

public interface IProdutoService
{
    Task<ProdutoResponse[]> ObterProdutos();
    Task<ProdutoResponse[]> ObterProdutosPorIds(Guid[] ids);
}
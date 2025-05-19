using backend.application.DTOs.responses;
namespace backend.application.services.contracts;

public interface IProdutoService
{
    Task<ProdutoResponse[]> ObterProdutosAsync();
    Task<ProdutoResponse[]> ObterProdutosPorIdsAsync(Guid[] ids);
}
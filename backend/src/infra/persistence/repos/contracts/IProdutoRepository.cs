using backend.domain;

namespace backend.infra.repos.contracts
{
    public interface IProdutoRepository
    {
        Task<Produto[]> ObterProdutosAsync();
        Task<Produto[]> ObterProdutosPorIdsAsync(HashSet<Guid> ids);
        
        Task<Produto?> ObterProdutoPorIdAsync(Guid id);
        Task<Produto> CriarProdutoAsync(Produto produto);
        Task<bool> SalvarAsync();
    }
}
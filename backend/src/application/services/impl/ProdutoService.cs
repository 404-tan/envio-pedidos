using backend.application.DTOs.responses;
using backend.application.services.contracts;
using backend.infra.repos.contracts;

namespace backend.application.services.impl;
public sealed class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<ProdutoResponse[]> ObterProdutosAsync()
    {
        var todosProdutos = await _produtoRepository.ObterProdutosAsync();

        return [.. todosProdutos.Select(p => new ProdutoResponse(p.Id, p.Nome, p.PrecoAtual))];
    }

    public async Task<ProdutoResponse[]> ObterProdutosPorIdsAsync(Guid[] ids)
    {
        var produtosFiltradosPorId = await _produtoRepository.ObterProdutosPorIdsAsync(ids.ToHashSet());
        return [.. produtosFiltradosPorId.Select(p => new ProdutoResponse(p.Id, p.Nome, p.PrecoAtual))];
    }
}
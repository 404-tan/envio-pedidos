using backend.domain;
using backend.infra.repos.contracts;
using Microsoft.EntityFrameworkCore;

namespace backend.infra.repos.impl;

public sealed class ProdutoRepository : IProdutoRepository
{
    private readonly PedidoContext _context;

    public ProdutoRepository(PedidoContext context)
    {
        _context = context;
    }

    public async Task<bool> SalvarAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
    public async Task<Produto> CriarProdutoAsync(Produto produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return produto;    
    }

    public async Task<Produto?> ObterProdutoPorIdAsync(Guid id)
    {
        return await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Produto[]> ObterProdutosAsync()
    {
        return await _context.Produtos.ToArrayAsync();
    }

    public async Task<Produto[]> ObterProdutosPorIdsAsync(HashSet<Guid> ids)
    {
        return await _context.Produtos.Where(p => ids.Contains(p.Id)).ToArrayAsync();
    }
}
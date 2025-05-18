using backend.domain;
using backend.infra.repos.contracts;
using backend.infra.repos.impl;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace backend.tests.infra;

public sealed class ProdutoRepositoryTest
{
    private readonly IProdutoRepository _produtoRepository;
    public ProdutoRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<PedidoContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new PedidoContext(options);
        _produtoRepository = new ProdutoRepository(context);
    }
    
    [Fact]
    public async Task CriarProdutoAsync_CriaUmNovoProduto()
    {
        var produto = Produto.Criar("Produto Teste", 25m);

        var resultado = await _produtoRepository.CriarProdutoAsync(produto);
        var produtoCriado = await _produtoRepository.ObterProdutoPorIdAsync(resultado.Id);

        produtoCriado.Should().NotBeNull();
        produtoCriado.Id.Should().NotBeEmpty();
        produtoCriado.Id.Should().Be(resultado.Id);
    }
     [Fact]
    public async Task ObterProdutoPorIdAsync_DeveRetornarProduto_QuandoExiste()
    {
        var produto = Produto.Criar("Produto Teste", 10m);
        await _produtoRepository.CriarProdutoAsync(produto);
 

        var resultado = await _produtoRepository.ObterProdutoPorIdAsync(produto.Id);

        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(produto.Id);
        resultado.Nome.Should().Be("Produto Teste");
    }

    [Fact]
    public async Task ObterProdutoPorIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        
        var resultado = await _produtoRepository.ObterProdutoPorIdAsync(Guid.NewGuid());

        resultado.Should().BeNull();
    }

    [Fact]
    public async Task ObterProdutosAsync_DeveRetornarTodosProdutos()
    {

        var produto1 = Produto.Criar("Produto 1", 10m);
        var produto2 = Produto.Criar("Produto 2", 20m);
        await _produtoRepository.CriarProdutoAsync(produto1);
        await _produtoRepository.CriarProdutoAsync(produto2);


        var resultado = await _produtoRepository.ObterProdutosAsync();

        resultado.Should().HaveCount(2);
        resultado.Select(p => p.Nome).Should().Contain(new[] { "Produto 1", "Produto 2" });
    }

    [Fact]
    public async Task ObterProdutosPorIdsAsync_DeveRetornarProdutosFiltrados()
    {
        var produto1 = Produto.Criar("Produto 1", 10m);
        var produto2 = Produto.Criar("Produto 2", 20m);
        var produto3 = Produto.Criar("Produto 3", 30m);
        await _produtoRepository.CriarProdutoAsync(produto1);
        await _produtoRepository.CriarProdutoAsync(produto2);
        await _produtoRepository.CriarProdutoAsync(produto3);
        var resultado = await _produtoRepository.ObterProdutosPorIdsAsync(new HashSet<Guid> { produto1.Id, produto3.Id });

        resultado.Should().HaveCount(2);
        resultado.Any(p => p.Id == produto1.Id).Should().BeTrue();
        resultado.Any(p => p.Id == produto3.Id).Should().BeTrue();
    }
}
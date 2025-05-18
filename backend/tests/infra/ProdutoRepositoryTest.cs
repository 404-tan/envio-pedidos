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
            .UseInMemoryDatabase("backend_test")
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

}
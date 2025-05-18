using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.application.DTOs.responses;
using backend.application.services.impl;
using backend.domain;
using backend.infra.repos.contracts;
using FluentAssertions;
using Moq;
using Xunit;

public class ProdutoServiceTest
{
    private readonly Mock<IProdutoRepository> _produtoRepositoryMock = new();

    private ProdutoService CriarService()
        => new(_produtoRepositoryMock.Object);

    [Fact]
    public async Task ObterProdutos_DeveRetornarTodosProdutos()
    {
        // Arrange
        var produtos = new List<Produto>
        {
            Produto.Criar("Produto 1", 10m),
            Produto.Criar("Produto 2", 20m)
        };

        _produtoRepositoryMock
            .Setup(x => x.ObterProdutosAsync())
            .ReturnsAsync(produtos.ToArray());

        var service = CriarService();

        // Act
        var result = await service.ObterProdutos();

        // Assert
        result.Should().HaveCount(2);
        result[0].Nome.Should().Be("Produto 1");
        result[1].PrecoAtual.Should().Be(20m);
    }

    [Fact]
    public async Task ObterProdutosPorIds_DeveRetornarProdutosFiltrados()
    {
        // Arrange

        var produtos = new List<Produto>
        {
            Produto.Criar("Produto 1", 10m),
            Produto.Criar("Produto 2", 20m)
        };
        var id1 = produtos[0].Id;
        var id2 = produtos[1].Id;
        _produtoRepositoryMock
            .Setup(x => x.ObterProdutosPorIdsAsync(It.IsAny<HashSet<Guid>>()))
            .ReturnsAsync([produtos.First()]);

        var service = CriarService();

        // Act
        var result = await service.ObterProdutosPorIds(new[] { id1 });

        // Assert
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(id1);
    }
}
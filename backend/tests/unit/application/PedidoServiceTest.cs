using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.application.DTOs.requests;
using backend.application.DTOs.responses;
using backend.application.services.contracts;
using backend.application.services.impl;
using backend.domain;
using backend.domain.enums;
using backend.infra.repos.contracts;
using backend.infra.security.contracts;
using FluentAssertions;
using Moq;
using Xunit;
namespace backend.tests.unit.application;
public class PedidoServiceTest
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock = new();
    private readonly Mock<IUsuarioService> _usuarioServiceMock = new();
    private readonly Mock<IProdutoService> _produtoServiceMock = new();

    private PedidoService CriarService()
        => new(_pedidoRepositoryMock.Object, _usuarioServiceMock.Object, _produtoServiceMock.Object);

    [Fact]
    public async Task CriarPedido_DeveRetornarPedidoResponse()
    {
        // Arrange
        var idCliente = Guid.NewGuid();
        var produtoDomain = Produto.Criar("Produto Teste", 10m);
        var itensRequest = new List<ItemPedidoRequest>
        {
            new(produtoDomain.Id,2)
        };
        var produtoResponse = new ProdutoResponse(produtoDomain.Id,"Produto Teste", 10m);
        
        var pedido = Pedido.Criar(idCliente, new List<(Guid, int, decimal)> { (produtoDomain.Id, 2, 10m) });
        pedido.Itens[0].ForcarProduto(produtoDomain);
        _usuarioServiceMock.Setup(x => x.ObterIdUsuarioAutenticado()).Returns(idCliente);
        _produtoServiceMock.Setup(x => x.ObterProdutosPorIdsAsync(It.IsAny<Guid[]>())).ReturnsAsync([produtoResponse]);
        _pedidoRepositoryMock.Setup(x => x.CriarPedidoAsync(It.IsAny<Pedido>())).ReturnsAsync(pedido.Id);
        _pedidoRepositoryMock.Setup(x => x.ObterPedidoComItensEProdutosPorIdAsync(pedido.Id)).ReturnsAsync(pedido);

        var service = CriarService();

        var request = new CriarPedidoRequest( itensRequest);

        // Act
        var response = await service.CriarPedidoAsync(request);

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(pedido.Id);
        response.IdCliente.Should().Be(idCliente);
        response.Total.Should().Be(20m);
        response.StatusAtual.Should().Be(PedidoStatus.Criado);
    }

    [Fact]
    public async Task CriarPedido_ProdutoNaoExiste_DeveLancarExcecao()
    {
        var idCliente = Guid.NewGuid();
        var idProduto = Guid.NewGuid();
        var itensRequest = new List<ItemPedidoRequest>
        {
            new(idProduto,2)
        };

        _usuarioServiceMock.Setup(x => x.ObterIdUsuarioAutenticado()).Returns(idCliente);
        _produtoServiceMock.Setup(x => x.ObterProdutosPorIdsAsync(It.IsAny<Guid[]>())).ReturnsAsync([]);

        var service = CriarService();
        var request = new CriarPedidoRequest(itensRequest);

        Func<Task> act = async () => await service.CriarPedidoAsync(request);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"O produto {idProduto} não existe.");
    }
    [Fact]
    public async Task ProcessarPedido_DeveRetornarProcessarPedidoResponse()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var pedidoId = Guid.NewGuid();
        var nomeAdmin = "Administrador Teste";
        var produtoNome = "Produto Teste";
        var produtoDomain = Produto.Criar("Produto Teste", 10m);
        var pedido = Pedido.Criar(usuarioId, new List<(Guid, int, decimal)> { (produtoDomain.Id, 1, 10m) });
        pedido.Itens[0].ForcarProduto(produtoDomain);
        _usuarioServiceMock.Setup(x => x.ObterIdUsuarioAutenticado()).Returns(usuarioId);
        _usuarioServiceMock.Setup(x => x.IsAdminAsync(usuarioId)).ReturnsAsync(true);
        _usuarioServiceMock.Setup(x => x.ObterNomeCompletoUsuarioPorIdAsync(usuarioId)).ReturnsAsync(nomeAdmin);
        _pedidoRepositoryMock.Setup(x => x.ObterPedidoComItensEProdutosPorIdAsync(pedidoId)).ReturnsAsync(pedido);
        _pedidoRepositoryMock.Setup(x => x.AtualizarStatusPedidoAsync(pedido)).ReturnsAsync(true);

        var service = CriarService();
        var request = new ProcessarPedidoRequest(pedidoId);

        // Act
        var response = await service.ProcessarPedidoAsync(request);

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(pedido.Id);
        response.IdCliente.Should().Be(pedido.IdCliente);
        response.NomeAdministradorProcessador.Should().Be(nomeAdmin);
        response.Itens.Should().HaveCount(1);
        response.Itens[0].NomeProduto.Should().Be(produtoNome);
        response.Total.Should().Be(pedido.Total);
        response.StatusAtual.Should().Be(pedido.StatusAtual);
        response.DataCriacao.Should().Be(pedido.DataCriacao);
        response.DataAtualizacao.Should().Be(pedido.DataAtualizacao);
    }

    [Fact]
    public async Task ProcessarPedido_UsuarioNaoAdmin_DeveLancarExcecao()
    {
        var usuarioId = Guid.NewGuid();
        var pedidoId = Guid.NewGuid();

        _usuarioServiceMock.Setup(x => x.ObterIdUsuarioAutenticado()).Returns(usuarioId);
        _usuarioServiceMock.Setup(x => x.IsAdminAsync(usuarioId)).ReturnsAsync(false);

        var service = CriarService();
        var request = new ProcessarPedidoRequest(pedidoId);

        Func<Task> act = async () => await service.ProcessarPedidoAsync(request);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Usuário não autorizado a processar pedidos.");
    }

    [Fact]
    public async Task ProcessarPedido_PedidoNaoEncontrado_DeveLancarExcecao()
    {
        var usuarioId = Guid.NewGuid();
        var pedidoId = Guid.NewGuid();

        _usuarioServiceMock.Setup(x => x.ObterIdUsuarioAutenticado()).Returns(usuarioId);
        _usuarioServiceMock.Setup(x => x.IsAdminAsync(usuarioId)).ReturnsAsync(true);
        _usuarioServiceMock.Setup(x => x.ObterNomeCompletoUsuarioPorIdAsync(usuarioId)).ReturnsAsync("Admin");
        _pedidoRepositoryMock.Setup(x => x.ObterPedidoComItensEProdutosPorIdAsync(pedidoId)).ReturnsAsync((Pedido?)null);

        var service = CriarService();
        var request = new ProcessarPedidoRequest(pedidoId);

        Func<Task> act = async () => await service.ProcessarPedidoAsync(request);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Pedido não encontrado.");
    }

    [Fact]
    public async Task ProcessarPedido_AtualizacaoFalha_DeveLancarExcecao()
    {
        var usuarioId = Guid.NewGuid();
        var pedidoId = Guid.NewGuid();
        var pedido = Pedido.Criar(usuarioId, new List<(Guid, int, decimal)> { (Guid.NewGuid(), 1, 10m) });

        _usuarioServiceMock.Setup(x => x.ObterIdUsuarioAutenticado()).Returns(usuarioId);
        _usuarioServiceMock.Setup(x => x.IsAdminAsync(usuarioId)).ReturnsAsync(true);
        _usuarioServiceMock.Setup(x => x.ObterNomeCompletoUsuarioPorIdAsync(usuarioId)).ReturnsAsync("Admin");
        _pedidoRepositoryMock.Setup(x => x.ObterPedidoComItensEProdutosPorIdAsync(pedidoId)).ReturnsAsync(pedido);
        _pedidoRepositoryMock.Setup(x => x.AtualizarStatusPedidoAsync(pedido)).ReturnsAsync(false);

        var service = CriarService();
        var request = new ProcessarPedidoRequest(pedidoId);

        Func<Task> act = async () => await service.ProcessarPedidoAsync(request);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Erro ao atualizar o pedido.");
    }
}
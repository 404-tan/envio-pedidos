using backend.domain;
using backend.infra.repos.contracts;
using backend.infra.repos.impl;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using backend.tests.helpers;
using backend.domain.enums;
namespace backend.tests.infra;


public class PedidoRepositoryTest
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<PedidoContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new PedidoContext(options);
        _pedidoRepository = new PedidoRepository(context);
    }
    [Fact]
    public async Task CriarPedidoAsync_CriaUmNovoPedido()
    {

        var pedido = Pedido.Criar(Guid.NewGuid(), [
            (Guid.NewGuid(), 2, 10m)
        ]);


        var idPedidoCriado = await _pedidoRepository.CriarPedidoAsync(pedido);
        var pedidoCriado = await _pedidoRepository.ObterPedidoComItensEProdutosPorIdAsync(idPedidoCriado);


        pedidoCriado.Should().NotBeNull();
        pedidoCriado.Id.Should().NotBeEmpty();
        pedidoCriado.Id.Should().Be(idPedidoCriado);
        pedidoCriado.Itens.Should().NotBeNull();
        pedidoCriado.Itens.Should().HaveCount(1);
        pedidoCriado.Itens.First().Id.Should().NotBeEmpty();
    }
    [Fact]
    public async Task GetPedidos_Por_Cursor_Deve_Retornar_Pedidos()
    {
        var pedidos = PedidoFactory.CriarListaDePedidos(100);
        foreach (var pedido in pedidos)
        {
            await _pedidoRepository.CriarPedidoAsync(pedido);
        }
        var cursor = DateTime.UtcNow.AddDays(1);
        var resultado = await _pedidoRepository.ObterPedidosPorCursorEStatusAsync(cursor, null, null, null, 10);
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(10);
        resultado.Should().AllBeOfType<Pedido>();
        resultado.Should().OnlyContain(p => p.DataCriacao < cursor);
        cursor = resultado.Last().DataCriacao;
        var ultimoId = resultado.Last().Id;
        var resultadoPagina2 = await _pedidoRepository.ObterPedidosPorCursorEStatusAsync(cursor, ultimoId, null, null, 10);
        resultadoPagina2.Should().NotBeNull();
        resultadoPagina2.Should().HaveCount(10);
        resultadoPagina2.Should().AllBeOfType<Pedido>();
        resultadoPagina2.Should().OnlyContain(p => p.DataCriacao < cursor);
    }
    [Fact]
    public async Task AtualizarPedidoAsync_AtualizaUmPedidoExistente()
    {
        var pedido = Pedido.Criar(Guid.NewGuid(), [
            (Guid.NewGuid(), 2, 10m)
        ]);
        var idPedidoCriado = await _pedidoRepository.CriarPedidoAsync(pedido);
        var pedidoCriado = await _pedidoRepository.ObterPedidoComItensEProdutosPorIdAsync(idPedidoCriado);
        Guid adminId = Guid.NewGuid();
        pedidoCriado!.Processar(adminId);
        var resultado = await _pedidoRepository.AtualizarStatusPedidoAsync(pedidoCriado);
        resultado.Should().BeTrue();
        var pedidoAtualizado = await _pedidoRepository.ObterPedidoComItensEProdutosPorIdAsync(pedidoCriado.Id);
        pedidoAtualizado.Should().NotBeNull();
        pedidoAtualizado.StatusAtual.Should().Be(PedidoStatus.Processado);
        pedidoAtualizado.HistoricoStatus.Should().NotBeNull();
        pedidoAtualizado.HistoricoStatus.Should().HaveCount(1);
        pedidoAtualizado.HistoricoStatus.First().Status.Should().Be(PedidoStatus.Criado);
        pedidoAtualizado.HistoricoStatus.First().DataAtualizacao.Should().Be(pedidoAtualizado.DataCriacao);
    }
    [Fact]
    public async Task ObterPedidosPorCursorEStatusAsync_DeveRetornarSomentePedidosComStatusCriado()
    {
        // Arrange
        var pedidosCriados = PedidoFactory.CriarListaDePedidos(5);
        var pedidosProcessados = PedidoFactory.CriarListaDePedidos(3);


        foreach (var pedido in pedidosProcessados)
        {
            await _pedidoRepository.CriarPedidoAsync(pedido);
            pedido.Processar(Guid.NewGuid());
            await _pedidoRepository.AtualizarStatusPedidoAsync(pedido);
        }


        foreach (var pedido in pedidosCriados)
        {
            await _pedidoRepository.CriarPedidoAsync(pedido);
        }

        var cursor = DateTime.UtcNow.AddDays(1);

        // Act
        var resultado = await _pedidoRepository.ObterPedidosPorCursorEStatusAsync(
            cursor, null, null, PedidoStatus.Criado, 20);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().OnlyContain(p => p.StatusAtual == PedidoStatus.Criado);
        resultado.Should().HaveCount(pedidosCriados.Count);
    }
}
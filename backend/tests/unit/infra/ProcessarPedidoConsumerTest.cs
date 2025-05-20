using System;
using System.Threading.Tasks;
using backend.application.commands;
using backend.domain;
using backend.infra.consumer;
using backend.infra.repos.contracts;
using MassTransit;
using Moq;
using Xunit;

public class ProcessarPedidoConsumerTest
{
    [Fact]
    public async Task Consume_DeveProcessarEPersistirPedido()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var adminId = Guid.NewGuid();
        var command = new ProcessarPedidoCommand(pedidoId, adminId, "Admin");

        var pedido = Pedido.Criar(Guid.NewGuid(), new() { (Guid.NewGuid(), 1, 10m) });

        var repoMock = new Mock<IPedidoRepository>();
        repoMock.Setup(r => r.ObterPedidoComItensEProdutosPorIdAsync(pedidoId)).ReturnsAsync(pedido);
        repoMock.Setup(r => r.AtualizarStatusPedidoAsync(pedido)).ReturnsAsync(true);

        var consumer = new ProcessarPedidoConsumer(repoMock.Object);

        var contextMock = new Mock<ConsumeContext<ProcessarPedidoCommand>>();
        contextMock.SetupGet(x => x.Message).Returns(command);

        // Act
        await consumer.Consume(contextMock.Object);

        // Assert
        repoMock.Verify(r => r.ObterPedidoComItensEProdutosPorIdAsync(pedidoId), Times.Once);
        repoMock.Verify(r => r.AtualizarStatusPedidoAsync(pedido), Times.Once);
        Assert.Equal(adminId, pedido.ProcessadoPorAdministradorId);
    }

    [Fact]
    public async Task Consume_PedidoNaoEncontrado_NaoProcessaENaoPersiste()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var adminId = Guid.NewGuid();
        var command = new ProcessarPedidoCommand(pedidoId, adminId, "Admin");

        var repoMock = new Mock<IPedidoRepository>();
        repoMock.Setup(r => r.ObterPedidoComItensEProdutosPorIdAsync(pedidoId)).ReturnsAsync((Pedido?)null);

        var consumer = new ProcessarPedidoConsumer(repoMock.Object);

        var contextMock = new Mock<ConsumeContext<ProcessarPedidoCommand>>();
        contextMock.SetupGet(x => x.Message).Returns(command);

        // Act
        await consumer.Consume(contextMock.Object);

        // Assert
        repoMock.Verify(r => r.ObterPedidoComItensEProdutosPorIdAsync(pedidoId), Times.Once);
        repoMock.Verify(r => r.AtualizarStatusPedidoAsync(It.IsAny<Pedido>()), Times.Never);
    }

    [Fact]
    public async Task Consume_AtualizacaoFalha_DeveLancarExcecao()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var adminId = Guid.NewGuid();
        var command = new ProcessarPedidoCommand(pedidoId, adminId, "Admin");

        var pedido = Pedido.Criar(Guid.NewGuid(), new() { (Guid.NewGuid(), 1, 10m) });

        var repoMock = new Mock<IPedidoRepository>();
        repoMock.Setup(r => r.ObterPedidoComItensEProdutosPorIdAsync(pedidoId)).ReturnsAsync(pedido);
        repoMock.Setup(r => r.AtualizarStatusPedidoAsync(pedido)).ReturnsAsync(false);

        var consumer = new ProcessarPedidoConsumer(repoMock.Object);

        var contextMock = new Mock<ConsumeContext<ProcessarPedidoCommand>>();
        contextMock.SetupGet(x => x.Message).Returns(command);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => consumer.Consume(contextMock.Object));
    }
}
using FluentAssertions;
using Xunit;
using backend.domain;
using backend.domain.enums;

namespace backend.tests.unit.domain;



public class PedidoTest
{
    [Fact]
    public void CriarPedido_DeveTerStatusCriado()
    {

        Guid IdCliente = Guid.NewGuid();
        var itens = new List<(Guid idProduto, int qtd, decimal preco)> {
            (Guid.NewGuid(), 2, 10),
            (Guid.NewGuid(), 1, 5)
        };

        var pedido = Pedido.Criar(IdCliente, itens);

        pedido.StatusAtual.Should().Be(PedidoStatus.Criado);
    }

    [Fact]
    public void CriarPedido_ComItens_DeveCalcularTotal()
    {

        Guid IdCliente = Guid.NewGuid();
        var itens = new List<(Guid idProduto, int qtd, decimal preco)> {
            (Guid.NewGuid(), 2, 10),
            (Guid.NewGuid(), 1, 5)
        };

        var pedido = Pedido.Criar(IdCliente, itens);

        pedido.Total.Should().Be(25);
    }

    [Fact]
    public void CriarPedido_SemItens_DeveLancarExcecao()
    {
        Guid IdCliente = Guid.NewGuid();
        Action act = () => Pedido.Criar(IdCliente, []);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Pedido deve conter ao menos um item.");
    }
    [Fact]
    public void ProcessarPedido_DeveAlterarStatus()
    {
        Guid IdCliente = Guid.NewGuid();
        var itens = new List<(Guid idProduto, int qtd, decimal preco)> {
            (Guid.NewGuid(), 2, 10),
            (Guid.NewGuid(), 1, 5)
        };

        var pedido = Pedido.Criar(IdCliente, itens);
        var AdministradorId = Guid.NewGuid();
        pedido.Processar(AdministradorId);

        pedido.StatusAtual.Should().Be(PedidoStatus.Processado);
    }
    [Fact]
    public void ProcessarPedido_ComStatusInvalido_DeveLancarExcecao()
    {
        Guid IdCliente = Guid.NewGuid();
        var itens = new List<(Guid idProduto, int qtd, decimal preco)> {
            (Guid.NewGuid(), 2, 10),
            (Guid.NewGuid(), 1, 5)
        };

        var pedido = Pedido.Criar(IdCliente, itens);
        var AdministradorId = Guid.NewGuid();
        pedido.Processar(AdministradorId);

        Action act = () => pedido.Processar(AdministradorId);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Pedido não pode ser processado.");
    }
    [Fact]
    public void ProcessarPedido_Deve_GerarHistorico()
    {
        Guid IdCliente = Guid.NewGuid();
        var itens = new List<(Guid idProduto, int qtd, decimal preco)> {
            (Guid.NewGuid(), 2, 10),
            (Guid.NewGuid(), 1, 5)
        };

        var pedido = Pedido.Criar(IdCliente, itens);
        var AdministradorId = Guid.NewGuid();
        pedido.Processar(AdministradorId);
        pedido.ProcessadoPorAdministradorId.Should().Be(AdministradorId);
        pedido.HistoricoStatus.Should().NotBeNull();
        pedido.HistoricoStatus.Should().HaveCount(1);
        pedido.HistoricoStatus.First().Status.Should().Be(PedidoStatus.Criado);
        pedido.HistoricoStatus.First().DataAtualizacao.Should().Be(pedido.DataCriacao);
        pedido.HistoricoStatus.First().UsuarioId.Should().Be(IdCliente);

    }
}

using FluentAssertions;
using Xunit;
using backend.domain;

namespace backend.tests.domain;


public class ItemPedidoTest
{
    [Fact]
    public void CriarItemPedido_Deve_Calcular_Total()
    {  
        ItemPedido item = ItemPedido.Criar(Guid.NewGuid(), Guid.NewGuid(), 10, 2);

        item.Total.Should().Be(20);
    }

    [Fact]
    public void CriarItemPedido_Com_QuantidadeZero_DeveLancarExcecao()
    {
        Guid IdCliente = Guid.NewGuid();
        Action act = () => ItemPedido.Criar(Guid.NewGuid(), Guid.NewGuid(), 10, 0);

        act.Should().Throw<ArgumentException>()
            .WithMessage("A quantidade deve ser maior que zero.");
    }
    [Fact]
    public void CriarItemPedido_Com_PrecoUnitarioZero_DeveLancarExcecao()
    {
        Guid IdCliente = Guid.NewGuid();
        Action act = () => ItemPedido.Criar(Guid.NewGuid(), Guid.NewGuid(), 0, 10);

        act.Should().Throw<ArgumentException>()
            .WithMessage("O preço unitário deve ser maior que zero.");
    }

}

using FluentAssertions;
using backend.domain;

namespace backend.tests.domain;


public class ProdutoTest
{
    [Fact]
    public void Deve_CriarProduto()
    {
        Produto produto = Produto.Criar("Produto Teste", 10);
        produto.Should().NotBeNull();
        produto.Nome.Should().Be("Produto Teste");
        produto.PrecoAtual.Should().Be(10);
    }

    [Fact]
    public void CriarProduto_Com_PrecoZero_DeveLancarExcecao()
    {
        Action act = () => Produto.Criar("Produto Teste", 0);

        act.Should().Throw<ArgumentException>()
            .WithMessage("O preço do produto deve ser maior que zero.");
    }
    [Fact]
    public void CriarProduto_Com_NomeVazio_DeveLancarExcecao()
    {

        Action act = () => Produto.Criar("", 10);
        act.Should().Throw<ArgumentException>()
            .WithMessage("O nome do produto não pode ser vazio.");

    }

}

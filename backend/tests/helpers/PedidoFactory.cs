using backend.domain;
using backend.domain.enums;

namespace backend.tests.helpers;

public class PedidoFactory
{
    public static List<Pedido> CriarListaDePedidos(int quantidade)
    {
        var pedidos = new List<Pedido>();
        for (int i = 0; i < quantidade; i++)
        {
            var pedido = Pedido.Criar(
                Guid.NewGuid(),
                new List<(Guid idProduto, int qtd, decimal preco)>
                {
                    (Guid.NewGuid(), 2, 10),
                    (Guid.NewGuid(), 1, 5)
                }
            );
            pedidos.Add(pedido);
        }
        return pedidos;
    }
}
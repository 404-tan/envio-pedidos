
using backend.domain.enums;
using Microsoft.AspNetCore.Mvc;

namespace backend.domain
{
    public sealed class Pedido
    {
        public Guid Id { get; private set; }
        public Guid IdCliente { get; private set; }
        public List<ItemPedido> Itens { get; private set; }
        public decimal Total => Itens.Sum(i => i.Total);
        public PedidoStatus StatusAtual { get; private set; }
        public HistoricoStatusPedido? HistoricoStatus { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public Usuario? Cliente { get; private set; }
        private Pedido(Guid idCliente)
        {
            Id = Guid.NewGuid();
            IdCliente = idCliente;
            Itens = [];
            StatusAtual = PedidoStatus.Criado;
            DataCriacao = DateTime.UtcNow;
        }

        public static Pedido Criar(Guid idCliente, List<(Guid idProduto, int qtd, decimal preco)> dadosDosItens)
        {
            if (dadosDosItens == null || !dadosDosItens.Any())
                throw new ArgumentException("Pedido deve conter ao menos um item.");

            var pedido = new Pedido(idCliente);
            var itens = dadosDosItens.Select(d =>
                ItemPedido.Criar(pedido.Id,d.idProduto,d.preco,d.qtd)).ToList();
            pedido.Itens = itens;
            return pedido;
        }
        public void Processar()
        {
            if (StatusAtual != PedidoStatus.Criado)
                throw new InvalidOperationException("Pedido não pode ser processado.");
            HistoricoStatus = new HistoricoStatusPedido(Id, StatusAtual);
            StatusAtual = PedidoStatus.Processado;
        }

    }
}
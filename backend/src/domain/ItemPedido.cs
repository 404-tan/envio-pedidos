namespace backend.domain
{
    public sealed class ItemPedido(Guid produtoId, Guid pedidoId, decimal precoUnitario, int quantidade)
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid ProdutoId { get; private set; } = produtoId;
        public Guid PedidoId { get; private set; } = pedidoId;
        public int Quantidade { get; private set; } = quantidade;
        public decimal PrecoUnitario { get; private set; } = precoUnitario;
        public Produto? Produto { get; private set; }
        public Pedido? Pedido { get; private set; }
        public decimal Total => Quantidade * PrecoUnitario;
        public static ItemPedido Criar(Guid pedidoId,Guid produtoId, decimal precoUnitario, int quantidade)
        {
            if (quantidade <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");
            if (precoUnitario <= 0)
                throw new ArgumentException("O preço unitário deve ser maior que zero.");

            return new ItemPedido(produtoId,pedidoId, precoUnitario, quantidade);
        }
    }
}
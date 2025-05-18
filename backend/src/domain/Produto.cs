namespace backend.domain
{
    public sealed class Produto
    {
        public Guid Id { get; }
        public string Nome { get; private set; }
        public decimal PrecoAtual { get; private set; }
        private Produto(string nome, decimal precoAtual)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            PrecoAtual = precoAtual;
        }
        public static Produto Criar(string nome, decimal precoAtual)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do produto não pode ser vazio.");
            if (precoAtual <= 0)
                throw new ArgumentException("O preço do produto deve ser maior que zero.");

            return new Produto(nome, precoAtual);
        }
    }
}
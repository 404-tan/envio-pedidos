namespace backend.domain
{
    public sealed class Produto(string nome, decimal precoAtual)
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Nome { get; private set; } = nome;
        public decimal PrecoAtual { get; private set; } = precoAtual;
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
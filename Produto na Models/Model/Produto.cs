namespace ProdutoCodeFirstAPI.Models
{
    public class Produto
    {
        public Guid Id { get; set; }
        public required string Nome { get; set; }
        public string? Marca { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
        public bool Ativo { get; set; }
    }
}

using ProdutoCodeFirstAPI.Interfaces;
using ProdutoCodeFirstAPI.Models;

namespace ProdutoCodeFirstAPI.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _repository;

        public ProdutoService(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Produto>> ObterTodosAsync()
        {
            return await _repository.ObterTodosAsync();
        }

        public async Task<Produto?> ObterPorIdAsync(Guid id)
        {
            return await _repository.ObterPorIdAsync(id);
        }

        public async Task<Produto> CriarAsync(Produto produto)
        {
            if (produto.Preco < 0)
                throw new ArgumentException("O preço não pode ser negativo.");

            if (produto.QuantidadeEstoque < 0)
                throw new ArgumentException("A quantidade em estoque não pode ser negativa.");

            return await _repository.CriarAsync(produto);
        }

        public async Task<Produto?> AtualizarAsync(Guid id, Produto produto)
        {
            if (id != produto.Id)
                throw new ArgumentException("O ID informado na rota é diferente do ID do produto.");

            if (produto.Preco < 0)
                throw new ArgumentException("O preço não pode ser negativo.");

            if (produto.QuantidadeEstoque < 0)
                throw new ArgumentException("A quantidade em estoque não pode ser negativa.");

            return await _repository.AtualizarAsync(produto);
        }

        public async Task<bool> DeletarAsync(Guid id)
        {
            return await _repository.DeletarAsync(id);
        }
    }
}

using ProdutoCodeFirstAPI.Models;

namespace ProdutoCodeFirstAPI.Interfaces
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> ObterTodosAsync();
        Task<Produto?> ObterPorIdAsync(Guid id);
        Task<Produto> CriarAsync(Produto produto);
        Task<Produto?> AtualizarAsync(Produto produto);
        Task<bool> DeletarAsync(Guid id);
    }
}

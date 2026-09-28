using Microsoft.EntityFrameworkCore;
using ProdutoCodeFirstAPI.Contexts;
using ProdutoCodeFirstAPI.Interfaces;
using ProdutoCodeFirstAPI.Models;

namespace ProdutoCodeFirstAPI.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ProdutoContext _context;

        public ProdutoRepository(ProdutoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produto>> ObterTodosAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task<Produto?> ObterPorIdAsync(Guid id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task<Produto> CriarAsync(Produto produto)
        {
            _context.Produtos.Add(produto);
            //nao sei desisto
            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task<Produto?> AtualizarAsync(Produto produto)
        {
            _context.Entry(produto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return produto;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProdutoExists(produto.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeletarAsync(Guid id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return false;
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> ProdutoExists(Guid id)
        {
            return await _context.Produtos.AnyAsync(e => e.Id == id);
        }
    }
}

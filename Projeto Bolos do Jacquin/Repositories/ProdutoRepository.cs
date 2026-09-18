using Microsoft.EntityFrameworkCore;
using Projeto_Bolos_do_Jacquin.BdContextBolos;
using Projeto_Bolos_do_Jacquin.Interfaces;
using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Repositories
{
    public class ProdutoRepository : IProduto
    {
        private readonly BolosContext _context;

        public ProdutoRepository(BolosContext context)
        {
            _context = context;
        }

        public async Task Cadastrar(Produtos produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<Produtos?> BuscarPorId(int id)
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.IdProdutos == id);
        }

        public async Task Atualizar(int id, Produtos produto)
        {
            var produtoBuscado = await _context.Produtos.FindAsync(id);

            if (produtoBuscado != null)
            {
                _context.Entry(produtoBuscado)
                    .CurrentValues
                    .SetValues(produto);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Deletar(int id)
        {
            var produtoBuscado = await _context.Produtos.FindAsync(id);

            if (produtoBuscado == null)
            {
                return false;
            }

            _context.Produtos.Remove(produtoBuscado);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Produtos>> Listar(
            string? nome,
            int? categoria,
            decimal? precoMin,
            decimal? precoMax)
        {
            var query = _context.Produtos
                .Include(p => p.Categoria)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(p =>
                    p.Nome.Contains(nome));
            }

            if (categoria.HasValue)
            {
                query = query.Where(p =>
                    p.IdCategoria == categoria.Value);
            }

            if (precoMin.HasValue)
            {
                query = query.Where(p =>
                    p.Preco >= precoMin.Value);
            }

            if (precoMax.HasValue)
            {
                query = query.Where(p =>
                    p.Preco <= precoMax.Value);
            }

            return await query.ToListAsync();
        }
    }
}
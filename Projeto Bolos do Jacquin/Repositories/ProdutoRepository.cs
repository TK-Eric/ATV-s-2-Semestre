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

        public async Task<Produtos?> BuscarPorId(Guid id)
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.IdProdutos == id);
        }

        public async Task Atualizar(Guid id, Produtos produto)
        {
            var produtoBuscado = await _context.Produtos.FindAsync(id);

            if (produtoBuscado != null)
            {
                // Copia os novos valores para a entidade encontrada
                _context.Entry(produtoBuscado).CurrentValues.SetValues(produto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Deletar(Guid id)
        {
            var produtoBuscado = await _context.Produtos.FindAsync(id);
            if (produtoBuscado != null)
            {
                _context.Produtos.Remove(produtoBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Produtos>> Listar()
        {
            return await _context.Produtos
                .Include(c => c.IdProdutos)
                .Include(c => c.Nome)
                .Include(c => c.Descricao)
                .Include(c => c.DescricaoLonga)
                .Include(c => c.DescricaoCurta)
                .Include(c => c.Categoria)
                .Include(c => c.Avaliacoes)
                .Include(c => c.Imagem)
                .Include(c => c.Preco)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
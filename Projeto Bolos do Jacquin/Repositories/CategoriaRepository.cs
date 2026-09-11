using Microsoft.EntityFrameworkCore;
using Projeto_Bolos_do_Jacquin.BdContextBolos;
using Projeto_Bolos_do_Jacquin.Interfaces;
using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Repositories
{
    public class CategoriaRepository : ICategoria
    {
        private readonly BolosContext _context;

        public CategoriaRepository(BolosContext context)
        {
            _context = context;
        }
        public async Task Atualizar(Guid id, Categorias categoria)
        {
            var categoriasBuscado = await _context.Categorias.FindAsync(id);

            if (categoriasBuscado != null)
            {
                // Copia os novos valores para a entidade encontrada
                _context.Entry(categoriasBuscado).CurrentValues.SetValues(categoria);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Categorias?> BuscarPorId(Guid id)
        {
            return await _context.Categorias
                .FirstOrDefaultAsync(c => c.IdCategorias == id);
        }

        public async Task Cadastrar(Categorias categoria)
        {
            
            await _context.Categorias.AddAsync(categoria);
            //esse metodo adiciona ele no banco, manipular o banco de dados
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var categoriasBuscado = await _context.Categorias.FindAsync(id);
            if (categoriasBuscado != null)
            {
                _context.Categorias.Remove(categoriasBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Categorias>> Listar()
        {
            return await _context.Categorias
                .Include(c => c.NomeCategoria)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}

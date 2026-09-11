using Microsoft.EntityFrameworkCore;
using Projeto_Bolos_do_Jacquin.BdContextBolos;
using Projeto_Bolos_do_Jacquin.Controller;
using Projeto_Bolos_do_Jacquin.Interfaces;
using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Repositories
{
    public class AvaliacaoRepository : IAvaliacao
    {
        private readonly BolosContext _context;

        public AvaliacaoRepository(BolosContext context)
        {
            _context = context;
        }

        public async Task Cadastrar(Avaliacoes avaliacoes)
        {
            avaliacoes.DataCriacao = DateTime.Now;
            await _context.Avaliacoes.AddAsync(avaliacoes);
            //esse metodo adiciona ele no banco, manipular o banco de dados
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var avaliacoesBuscado = await _context.Avaliacoes.FindAsync(id);
            if (avaliacoesBuscado != null)
            {
                _context.Avaliacoes.Remove(avaliacoesBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Avaliacoes>> Listar()
        {
            return await _context.Avaliacoes
                .Include(c => c.IdProdutos)
                .Include(c => c.IdUsuarios)
                .Include(c => c.DataCriacao)
                .AsNoTracking()
                .ToListAsync();
        }

        

        public async Task Atualizar(Guid id, Avaliacoes avaliacoes)
        {
            var avaliacoesBuscado = await _context.Avaliacoes.FindAsync(id);

            if (avaliacoesBuscado != null)
            {
                // Copia os novos valores para a entidade encontrada
                _context.Entry(avaliacoesBuscado).CurrentValues.SetValues(avaliacoes);
                await _context.SaveChangesAsync();
            }
        }

        public Task<List<Avaliacoes>> ListarPorProduto(Guid idProduto)
        {
            throw new NotImplementedException();
        }

        public Task<List<Avaliacoes>> ListarPorUsuario(Guid idUsuario)
        {
            throw new NotImplementedException();
        }

        public async Task<Avaliacoes> BuscarPorId(Guid id)
        {
            return await _context.Avaliacoes
                .Include(c => c.IdProdutos)
                .Include(c => c.IdUsuarios)
                .Include(c => c.DataCriacao)
                .FirstOrDefaultAsync(c => c.IdAvaliacoes == id);
        }
    }
}
}

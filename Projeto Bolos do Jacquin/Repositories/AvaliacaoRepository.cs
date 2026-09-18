using Microsoft.EntityFrameworkCore;
using Projeto_Bolos_do_Jacquin.BdContextBolos;
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

        public async Task Cadastrar(Avaliacoes avaliacao)
        {
            avaliacao.DataCriacao = DateTime.UtcNow;

            await _context.Avaliacoes.AddAsync(avaliacao);

            await _context.SaveChangesAsync();
        }

        public async Task Deletar(int id)
        {
            var avaliacaoBuscada =
                await _context.Avaliacoes.FindAsync(id);

            if (avaliacaoBuscada == null)
            {
                return;
            }

            _context.Avaliacoes.Remove(avaliacaoBuscada);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Avaliacoes>> Listar()
        {
            return await _context.Avaliacoes
                .Include(a => a.Produto)
                .Include(a => a.Usuario)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Avaliacoes>> ListarPorProduto(
            int idProduto)
        {
            return await _context.Avaliacoes
                .Where(a =>
                    a.IdProdutos == idProduto &&
                    a.Exibe)
                .Include(a => a.Usuario)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Avaliacoes>> ListarPorUsuario(
            int idUsuario)
        {
            return await _context.Avaliacoes
                .Where(a => a.IdUsuarios == idUsuario)
                .Include(a => a.Produto)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Avaliacoes?> BuscarPorId(int id)
        {
            return await _context.Avaliacoes
                .Include(a => a.Produto)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a =>
                    a.IdAvaliacoes == id);
        }

        public async Task Atualizar(
            int id,
            Avaliacoes avaliacao)
        {
            var avaliacaoBuscada =
                await _context.Avaliacoes.FindAsync(id);

            if (avaliacaoBuscada == null)
            {
                return;
            }

            avaliacaoBuscada.Nota = avaliacao.Nota;

            avaliacaoBuscada.Comentario =
                avaliacao.Comentario;

            avaliacaoBuscada.DataUltimaAlteracao =
                DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
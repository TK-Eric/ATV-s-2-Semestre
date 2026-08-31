using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class ComentarioRepository : IComentario
    {
        private readonly EventContext _context;

        public ComentarioRepository(EventContext context)
        {
            _context = context;
        }

        public async Task Cadastrar(Comentario comentario)
        {
            comentario.DataComentario = DateTime.Now;
            await _context.Comentario.AddAsync(comentario);
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var comentarioBuscado = await _context.Comentario.FindAsync(id);
            if (comentarioBuscado != null)
            {
                _context.Comentario.Remove(comentarioBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Comentario>> Listar()
        {
            return await _context.Comentario
                .Include(c => c.IdEventoNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Comentario>> ListarPorEvento(Guid idEvento)
        {
            return await _context.Comentario
                .Where(c => c.IdEvento == idEvento && c.Exibe == true)
                .Include(c => c.IdUsuarioNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Comentario?> BuscarPorId(Guid id)
        {
            return await _context.Comentario
                .Include(c => c.IdEventoNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(c => c.IdComentario == id);
        }

        public async Task Atualizar(Guid id, Comentario comentario)
        {
            var comentarioBuscado = await _context.Comentario.FindAsync(id);

            if (comentarioBuscado != null)
            {
                // Copia os novos valores para a entidade encontrada
                _context.Entry(comentarioBuscado).CurrentValues.SetValues(comentario);
                await _context.SaveChangesAsync();
            }
        }
    }
}
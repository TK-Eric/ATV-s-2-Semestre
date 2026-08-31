using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class PresencaRepository : IPresenca
    {
        private readonly EventContext _context;

        public PresencaRepository(EventContext context)
        {
            _context = context;
        }

        public async Task Inscrever(Presenca presenca)
        {
            await _context.Presenca.AddAsync(presenca);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarSituacao(Guid id, bool situacao)
        {
            var presencaBuscada = await _context.Presenca.FindAsync(id);
            if (presencaBuscada != null)
            {
                presencaBuscada.Situacao = situacao;
                _context.Presenca.Update(presencaBuscada);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Deletar(Guid id)
        {
            var presencaBuscada = await _context.Presenca.FindAsync(id);
            if (presencaBuscada != null)
            {
                _context.Presenca.Remove(presencaBuscada);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Presenca>> Listar()
        {
            return await _context.Presenca
                .Include(p => p.IdEventoNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Presenca>> ListarMinhasPresencas(Guid idUsuario)
        {
            return await _context.Presenca
                .Where(p => p.IdUsuario == idUsuario)
                .Include(p => p.IdEventoNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Presenca?> BuscarPorId(Guid id)
        {
            return await _context.Presenca
                .Include(p => p.IdEventoNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .FirstOrDefaultAsync(p => p.IdPresenca == id);
        }
    }
}
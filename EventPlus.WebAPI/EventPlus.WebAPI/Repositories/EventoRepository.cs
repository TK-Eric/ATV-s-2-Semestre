using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class EventoRepository : IEvento
    {
        private readonly EventContext _context;

        public EventoRepository(EventContext context)
        {
            _context = context;
        }

        public async Task Cadastrar(Evento evento)
        {
            await _context.Evento.AddAsync(evento);
            await _context.SaveChangesAsync();
        }

        public async Task Atualizar(Guid id, Evento evento)
        {
            var eventoBuscado = await _context.Evento.FindAsync(id);
            if (eventoBuscado != null)
            {
                eventoBuscado.NomeEvento = evento.NomeEvento;
                eventoBuscado.Descricao = evento.Descricao;
                eventoBuscado.DataEvento = evento.DataEvento;
                eventoBuscado.IdTipoEvento = evento.IdTipoEvento;
                eventoBuscado.IdInstituicao = evento.IdInstituicao;



                if (!string.IsNullOrEmpty(evento.ImagemUrl))
                {
                    eventoBuscado.ImagemUrl = evento.ImagemUrl;
                }

                _context.Evento.Update(eventoBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Deletar(Guid id)
        {
            var eventoBuscado = await _context.Evento.FindAsync(id);
            if (eventoBuscado != null)
            {
                _context.Evento.Remove(eventoBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Evento>> Listar()
        {
            return await _context.Evento
                .Include(e => e.IdTipoEventoNavigation)
                .Include(e => e.IdInstituicaoNavigation)
                .OrderBy(e => e.DataEvento)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Evento>> ListarProximos()
        {
            return await _context.Evento
                .Where(e => e.DataEvento >= DateTime.Now)
                .Include(e => e.IdTipoEventoNavigation)
                .Include(e => e.IdInstituicaoNavigation)
                .OrderBy(e => e.DataEvento)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Evento?> BuscarPorId(Guid id)
        {
            return await _context.Evento
                .Include(e => e.IdTipoEventoNavigation)
                .Include(e => e.IdInstituicaoNavigation)
                .FirstOrDefaultAsync(e => e.IdEvento == id);
        }

        Task<Evento> IEvento.Cadastrar(Evento evento)
        {
            throw new NotImplementedException();
        }
    }
}
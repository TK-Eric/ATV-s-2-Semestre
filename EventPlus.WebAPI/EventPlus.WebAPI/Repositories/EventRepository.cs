using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class EventoRepository : IEvent
    {
        private readonly EventContext _context;

        public EventoRepository(EventContext context)
        {
            _context = context;
        }


        public async Task Atualizar(Guid id, Evento eventoAtualizado)
        {
            var e = await _context.Evento.FindAsync(id);

            if (e != null)
            {
                e.NomeEvento = string.IsNullOrEmpty(eventoAtualizado.NomeEvento) ? e.NomeEvento : eventoAtualizado.NomeEvento;
                e.DataEvento = eventoAtualizado.DataEvento == DateTime.MinValue ? e.DataEvento : eventoAtualizado.DataEvento;
                e.Descricao = string.IsNullOrEmpty(eventoAtualizado.Descricao) ? e.Descricao : eventoAtualizado.Descricao;
                e.ImagemUrl = string.IsNullOrEmpty(eventoAtualizado.ImagemUrl) ? e.ImagemUrl : eventoAtualizado.ImagemUrl;
                e.IdTipoEvento = eventoAtualizado.IdTipoEvento == null ? e.IdTipoEvento : eventoAtualizado.IdTipoEvento;
                e.IdInstituicao = eventoAtualizado.IdInstituicao == null ? e.IdInstituicao : eventoAtualizado.IdInstituicao;

                _context.Evento.Update(e);
                await _context.SaveChangesAsync();
            }
        }

        public Task Atualizar(object id, Evento evento)
        {
            throw new NotImplementedException();
        }

        public async Task<Evento?> BuscarPorId(Guid id) => await _context.Evento.FindAsync(id);

        public async Task<Evento> Cadastrar(Evento e)
        {
            await _context.Evento.AddAsync(e);
            await _context.SaveChangesAsync();
            return await _context.Evento.FindAsync(e.IdEvento);
        }

        public async Task Deletar(Guid id)
        {
            var e = await _context.Evento.FindAsync(id);

            if (e != null)
            {
                _context.Evento.Remove(e);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Evento>> Listar()
        {
            return await _context.Evento.AsNoTracking().ToListAsync();
        }

        public async Task<List<Evento>> ListarProximos()
        {
            throw new NotImplementedException();
        }

       
    }
}
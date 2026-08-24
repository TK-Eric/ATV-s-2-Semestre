using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Repositories
{
    public class ComentarioRepository : IComentario
    {
        private readonly EventContext _context;

        public ComentarioRepository(EventContext context)
        {
            _context = context;
        }

        public Task<Comentario?> BuscarPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task Cadastrar(Comentario comentario)
        {
            comentario.DataComentario = DateTime.Now;
            await _context.Comentario.AddAsync(comentario);
            await _context.SaveChangesAsync();
        }

        public Task Deletar(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Comentario>> Listar()
        {
            throw new NotImplementedException();
        }

        public Task<List<Comentario>> ListarPorEvento(Guid idEvento)
        {
            throw new NotImplementedException();
        }
    }
}

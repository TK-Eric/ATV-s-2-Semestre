using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class TipoEventoRepository : ITipoEvento
    {

        private readonly EventContext _context;

        public TipoEventoRepository(EventContext context)
        {
            _context = context;
        }

        // Guid id : id do objeto buscado
        // TipoEvento novoTipo: objeto com as novas informações
        // TipoEvento: Classe
        // novoTipo: Objeto dessa classe
        // TituloTipoEvento: Propriedade do objeto
        public async Task Atualizar(Guid id, TipoEvento novoTipo)
        {
            // variável que guarda o resultado da busca(o objeto buscado que nós queremos "trocar" pelo novoTipo
            var tipoBuscado = await _context.TipoEvento.FindAsync(id);
            // a resposta dessa busca será null ou então o objeto encontrado

            // se o tipoBuscado existir
            if (tipoBuscado != null)
            {
                tipoBuscado.TituloTipoEvento = novoTipo.TituloTipoEvento;
                //substituir o titulo do objeto buscado pelo titulo do novoTipo

                _context.TipoEvento.Update(tipoBuscado);

                await _context.SaveChangesAsync();
            }
        }

        public Task Atualizar(Guid id, TipoEvento tipoEvento, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<TipoEvento?> BuscarPorId(Guid id)
        {
            return await _context.TipoEvento.FirstOrDefaultAsync(t => t.IdTipoEvento == id);
        }

        public async Task Cadastrar(TipoEvento tipoEvento)
        {
            await _context.TipoEvento.AddAsync(tipoEvento);
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var tipoBuscado = await _context.TipoEvento.FindAsync(id);

            //alternativa: var tipoBuscado = await _context.TipoEvento.FirstOrDefaultAsync(t => t.IdTipoEvento == id);

            if (tipoBuscado != null)
            {
                _context.TipoEvento.Remove(tipoBuscado);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TipoEvento>> Listar()
        {
            return await _context.TipoEvento.AsNoTracking().ToListAsync();
        }
    }
}
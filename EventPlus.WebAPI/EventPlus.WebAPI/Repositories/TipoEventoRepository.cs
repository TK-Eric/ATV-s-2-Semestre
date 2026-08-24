using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class TipoEventoRepository : ITipoEvento
    {
        // Contexto do EF Core que representa a conexão com o banco de dados.
        private readonly EventContext _dbContext;

        // Construtor com injeção de dependência: o EventContext é fornecido
        // automaticamente pelo container de DI do ASP.NET Core.
        public TipoEventoRepository(EventContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Deveria atualizar um TipoUsuario existente, identificado pelo Guid (id).
        // Ainda não implementado — lança exceção se for chamado.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tipoEvento"></param>
        /// <returns></returns>
        //public async Task Atualizar(Guid id, TipoEvento tipoEvento, CancellationToken cancellationToken)
        //{
        //    var tipoUsuarioBuscado = await
        //                   _dbContext.TipoEvento.FindAsync(id);
        //    if (tipoUsuarioBuscado != null)
        //    {
        //        tipoUsuarioBuscado.TituloTipoEvento =
        //            tipoEvento.TituloTipoEvento;
        //        _dbContext.TipoEvento.Update(tipoUsuarioBuscado);
        //        await _dbContext.SaveChangesAsync();
        //    }
        //}

 

        public async Task Atualizar(Guid id, TipoEvento tipoEvento, CancellationToken cancellationToken)
        {
            var tipoUsuarioBuscado = await _dbContext.TipoEvento.FindAsync(id);
            {
                if (tipoUsuarioBuscado != null)
                {
                    tipoUsuarioBuscado.TituloTipoEvento = tipoEvento.TituloTipoEvento;
                    _dbContext.Update(tipoUsuarioBuscado);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        // Deveria buscar um único TipoUsuario pelo seu Id (Guid).
        // Ainda não implementado.
        public async Task<TipoEvento?> BuscarPorId(Guid id)
        {
            return await _dbContext.TipoEvento.FirstOrDefaultAsync(t => t.IdTipoEvento == id);
        }

        // Deveria cadastrar (inserir) um novo TipoUsuario no banco.
        // Ainda não implementado.
        public async Task Cadastrar(TipoEvento tipoEvento)
        {
            await _dbContext.TipoEvento.AddAsync(tipoEvento);

            await _dbContext.SaveChangesAsync();
        }

        // Deveria deletar um TipoUsuario existente pelo Id.
        // Ainda não implementado.
        public async Task Deletar(Guid Id)
        {
            var tipoEventoBuscado = await
            _dbContext.TipoEvento.FindAsync(Id);
            if (tipoEventoBuscado != null)
            {
                _dbContext.TipoEvento.Remove(tipoEventoBuscado);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Único método realmente implementado até agora:
        // Retorna todos os registros de TipoUsuario do banco de dados.
        // AsNoTracking() melhora a performance porque diz ao EF Core que
        // esses dados são só leitura (não serão alterados/rastreados),
        // ideal para consultas de listagem.
        public async Task<List<TipoEvento>> Listar()
        {
            return await _dbContext.TipoEvento.AsNoTracking().ToListAsync();
        }

        Task ITipoEvento.Atualizar(Guid id, TipoEvento tipoEvento)
        {
            throw new NotImplementedException();
        }
    }
}

using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    // Classe responsável por acessar e manipular os dados de TipoUsuario no banco de dados.
    // Implementa a interface ITipoUsuario, garantindo que siga o "contrato" esperado
    // (útil para injeção de dependência e testes com mocks).
    public class TipoUsuarioRepository : ITipoUsuario
    {
        // Contexto do EF Core que representa a conexão com o banco de dados.
        private readonly EventContext _dbContext;

        // Construtor com injeção de dependência: o EventContext é fornecido
        // automaticamente pelo container de DI do ASP.NET Core.
        public TipoUsuarioRepository(EventContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Deveria atualizar um TipoUsuario existente, identificado pelo Guid (id).
        // Ainda não implementado — lança exceção se for chamado.
        public async Task Atualizar(Guid id, TipoUsuario tipoUsuario, CancellationToken cancellationToken)
        {
            var tipoUsuarioBuscado = await
                           _dbContext.TipoUsuario.FindAsync(id);
            if (tipoUsuarioBuscado != null)
            {
                tipoUsuarioBuscado.TituloTipoUsuario =
                    tipoUsuario.TituloTipoUsuario;
                _dbContext.TipoUsuario.Update(tipoUsuarioBuscado);
                await _dbContext.SaveChangesAsync();
            }
                }

        public async Task Atualizar(Guid id, TipoUsuario tipoUsuario)
        {
            var tipoUsuarioBuscado = await _dbContext.TipoUsuario.FindAsync(id);
            {
                if (tipoUsuarioBuscado != null)
                {
                    tipoUsuarioBuscado.TituloTipoUsuario = tipoUsuario.TituloTipoUsuario;
                    _dbContext.Update(tipoUsuarioBuscado);
                    await _dbContext.SaveChangesAsync();
                }
            }

        }

        // Deveria buscar um único TipoUsuario pelo seu Id (Guid).
        // Ainda não implementado.
        public async Task<TipoUsuario?> BuscarPorId(Guid id)
        {
            return await _dbContext.TipoUsuario.FirstOrDefaultAsync(t => t.IdTipoUsuario == id);
        }

        // Deveria cadastrar (inserir) um novo TipoUsuario no banco.
        // Ainda não implementado.
        public async Task Cadastrar(TipoUsuario tipoUsuario)
        {
             await _dbContext.TipoUsuario.AddAsync(tipoUsuario);

            await _dbContext.SaveChangesAsync();
        }

        // Deveria deletar um TipoUsuario existente pelo Id.
        // Ainda não implementado.
        public async Task Deletar(Guid Id)
        {
            var tipoUsuarioBuscado = await
            _dbContext.TipoUsuario.FindAsync(Id);
            if(tipoUsuarioBuscado != null)
            {
                _dbContext.TipoUsuario.Remove(tipoUsuarioBuscado);
                await _dbContext.SaveChangesAsync();
            }
        }

        // Único método realmente implementado até agora:
        // Retorna todos os registros de TipoUsuario do banco de dados.
        // AsNoTracking() melhora a performance porque diz ao EF Core que
        // esses dados são só leitura (não serão alterados/rastreados),
        // ideal para consultas de listagem.
        public async Task<List<TipoUsuario>> Listar()
        {
            return await _dbContext.TipoUsuario.AsNoTracking().ToListAsync();
        }
    }
}
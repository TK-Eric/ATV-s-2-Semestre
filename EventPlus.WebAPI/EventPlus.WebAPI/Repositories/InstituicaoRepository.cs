using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class InstituicaoRepository : IInstituicao
    {
        private readonly EventContext _dbContext;

        public InstituicaoRepository(EventContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Instituicao>> Listar()
        {
            return await _dbContext.Instituicao.AsNoTracking().ToListAsync();
        }

        public async Task<Instituicao?> BuscarPorId(Guid id)
        {
            return await _dbContext.Instituicao
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.IdInstituicao == id);
        }

        public async Task Cadastrar(Instituicao instituicao)
        {
            await _dbContext.Instituicao.AddAsync(instituicao);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Atualizar(Guid id, Instituicao instituicao)
        {
            var instituicaoBuscada = await _dbContext.Instituicao.FindAsync(id);

            if (instituicaoBuscada != null)
            {
                // Copia automaticamente todas as propriedades alteradas sem sobrescrever o ID
                _dbContext.Entry(instituicaoBuscada).CurrentValues.SetValues(instituicao);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task Deletar(Guid id)
        {
            var instituicaoBuscada = await _dbContext.Instituicao.FindAsync(id);

            if (instituicaoBuscada != null)
            {
                _dbContext.Instituicao.Remove(instituicaoBuscada);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
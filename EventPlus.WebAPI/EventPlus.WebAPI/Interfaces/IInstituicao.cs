using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IInstituicao
    {
        Task Cadastrar(Instituicao instituicao);
        Task<List<Instituicao>> Listar();
        Task Deletar(Guid Id);
        Task Atualizar(Guid id, Instituicao instituicao, CancellationToken cancellationToken);
        Task<List<Instituicao>> BuscarPorId(Guid id);
        Task<List<Instituicao>> ListarPorInstituicao(Guid id);
        Task<List<Instituicao>> ListarPorInscrito(Guid id);

        Task<TipoEvento?> ListarProximosEventos();
        Task Atualizar(Guid id, Instituicao instituicao);
        Task Atualizar(Guid id, IInstituicao instituicao);
    }
}

using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface ITipoEvento
    {
        Task Cadastrar(TipoEvento tipoEvento);
        Task<List<TipoEvento>> Listar();
        Task Deletar(Guid Id);
        Task Atualizar(Guid id, TipoEvento tipoEvento, CancellationToken cancellationToken);
        Task<TipoEvento?> BuscarPorId(Guid id);
        Task Atualizar(Guid id, TipoEvento tipoEvento);
    }
}

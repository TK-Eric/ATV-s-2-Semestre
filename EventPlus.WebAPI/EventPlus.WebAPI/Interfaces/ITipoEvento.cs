using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface ITipoEvento
    {
        Task Cadastrar(TipoEvento tipoEvento);
        Task Atualizar(Guid id, TipoEvento tipoEvento);
        Task Deletar(Guid id);
        Task<List<TipoEvento>> Listar();
        Task<TipoEvento?> BuscarPorId(Guid id);
    }
}
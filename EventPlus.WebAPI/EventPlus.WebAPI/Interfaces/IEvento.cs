using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IEvento
    {
        Task<Evento> Cadastrar(Evento evento);
        Task Atualizar(Guid id, Evento evento);
        Task Deletar(Guid id);
        Task<List<Evento>> Listar();
        Task<List<Evento>> ListarProximos();
        Task<Evento?> BuscarPorId(Guid id);
    }
}

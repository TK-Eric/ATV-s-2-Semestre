using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IComentario
    {
        Task Cadastrar(Comentario comentario);
        Task Deletar(Guid id);
        Task<List<Comentario>> Listar(Guid idEvento);
        Task<List<Comentario>> ListarPorEvento(Guid idEvento);
        Task<Comentario?> BuscarPorId(Guid id);
        Task<object?> ListarPorUsuario(Guid idUsuario);
        Task<object?> Listar();
    }
}

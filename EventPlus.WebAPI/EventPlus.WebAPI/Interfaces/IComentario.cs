using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IComentario
    {
        Task Cadastrar(Comentario comentario);
        Task Deletar(Guid id);
        Task<List<Comentario>> Listar();
        Task<List<Comentario>> ListarPorEvento(Guid idEvento);
        Task<Comentario?> BuscarPorId(Guid id);
        Task Atualizar(Guid id, Comentario evento);
    }
}
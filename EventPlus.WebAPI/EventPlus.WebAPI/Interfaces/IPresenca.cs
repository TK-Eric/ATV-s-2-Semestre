using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IPresenca
    {
        Task Inscrever(Presenca presenca);
        Task AtualizarSituacao(Guid id, bool situacao);
        Task Deletar(Guid id);
        Task<List<Presenca>> Listar();
        Task<List<Presenca>> ListarMinhasPresencas(Guid idUsuario);
        Task<Presenca?> BuscarPorId(Guid id);
    }
}
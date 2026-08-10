using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface ITipoUsuario
    {
        Task Cadastrar(TipoUsuario tipoUsuario);
        Task<List<TipoUsuario>> Listar();
        Task Deletar(Guid Id);
        Task Atualizar(Guid id, TipoUsuario tipoUsuario, CancellationToken cancellationToken);

        Task<TipoUsuario?> BuscarPorId(Guid id);
        Task Atualizar(Guid id, TipoUsuario tipoUsuario);
    }
}

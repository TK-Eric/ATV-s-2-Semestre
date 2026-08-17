using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    public interface IUsuario
    {
        Task Cadastrar(Usuario usuario);
        Task<List<Usuario>> Listar();
        Task Deletar(Guid Id);
        Task<Usuario?> BuscarPorEmailESenha(string email, string senha);

        Task<Usuario?> BuscarPorId(Guid id);
        Task Atualizar(Guid id, Usuario usuario);
    }
}

using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Interfaces
{
    public interface IUsuario
    {
        Task Cadastrar(Usuarios usuario);
        Task<List<Usuarios>> Listar();
        Task Deletar(Guid Id);
        Task<Usuarios?> BuscarPorEmailESenha(string email, string senha);

        Task<Usuarios?> BuscarPorId(Guid id);
        Task Atualizar(Guid id, Usuarios usuario);
    }
}

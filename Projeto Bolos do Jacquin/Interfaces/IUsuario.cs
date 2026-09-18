using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Interfaces
{
    public interface IUsuario
    {
        Task Cadastrar(Usuarios usuario);

        Task<List<Usuarios>> Listar();

        Task<bool> Deletar(int id);

        Task<Usuarios?> BuscarPorEmailESenha(
            string email,
            string senha);

        Task<Usuarios?> BuscarPorId(int id);

        Task<bool> EmailExiste(
            string email,
            int? idIgnorar = null);

        Task Atualizar(
            int id,
            Usuarios usuario);
    }
}
using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Interfaces
{
    public interface IAvaliacao
    {
        Task Cadastrar(Avaliacoes avaliacao);

        Task Deletar(int id);

        Task<List<Avaliacoes>> Listar();

        Task<List<Avaliacoes>> ListarPorProduto(int idProduto);

        Task<List<Avaliacoes>> ListarPorUsuario(int idUsuario);

        Task<Avaliacoes?> BuscarPorId(int id);

        Task Atualizar(int id, Avaliacoes avaliacao);
    }
}
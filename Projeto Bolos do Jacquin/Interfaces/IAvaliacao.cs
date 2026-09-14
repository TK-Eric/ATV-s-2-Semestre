using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Interfaces
{
    public interface IAvaliacao
    {
        Task Cadastrar(Avaliacoes avaliacoes);
        Task Deletar(int id);
        Task<List<Avaliacoes>> Listar();
        Task<List<Avaliacoes>> ListarPorProduto(int idProduto);
        Task<List<Avaliacoes>> ListarPorUsuario(int idUsuario);
        //se pa nao faço isso ai nao
        Task<Avaliacoes?> BuscarPorId(int id);
        Task Atualizar(int id, Avaliacoes avaliacoes);
    }
}

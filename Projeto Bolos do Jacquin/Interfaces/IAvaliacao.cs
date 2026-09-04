using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Interfaces
{
    public interface IAvaliacao
    {
        Task Cadastrar(Avaliacoes avaliacoes);
        Task Deletar(Guid id);
        Task<List<Avaliacoes>> Listar();
        Task<List<Avaliacoes>> ListarPorProduto(Guid idProduto);
        Task<List<Avaliacoes>> ListarPorUsuario(Guid idUsuario);
        //se pa nao faço isso ai nao
        Task<Avaliacoes?> BuscarPorId(Guid id);
        Task Atualizar(Guid id, Avaliacoes avaliacoes);
    }
}

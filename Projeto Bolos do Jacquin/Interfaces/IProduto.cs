using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Interfaces
{
    public interface IProduto
    {
        Task Cadastrar(Produtos produto);
        Task Deletar(Guid id);
        Task<List<Produtos>> Listar();
        Task<List<Produtos>> ListarPorEvento(Guid idEvento);
        Task<Produtos?> BuscarPorId(Guid id);
        Task Atualizar(Guid id, Produtos produto);
        Task AtualizarDisponibilidade(Guid id, Produtos produto);
    }
}

using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Interfaces
{
    public interface IProduto
    {
        Task Cadastrar(Produtos produto);
        Task Atualizar(Guid id, Produtos produto);
        Task Deletar(Guid id);
        Task<List<Produtos>> Listar();
        Task<Produtos?> BuscarPorId(Guid id);
    }
}
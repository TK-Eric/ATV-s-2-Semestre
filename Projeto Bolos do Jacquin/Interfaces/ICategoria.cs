using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Interfaces
{
    public interface ICategoria
    {
        Task Cadastrar(Categorias categoria);
        Task Deletar(Guid id);
        Task<List<Categorias>> Listar();
        Task<Categorias?> BuscarPorId(Guid id);
        Task Atualizar(Guid id, Categorias categoria);
    }
}

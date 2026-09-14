using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Interfaces
{
    public interface ICategoria
    {
        Task Cadastrar(Categorias categoria);
        Task Deletar(int id);
        Task<List<Categorias>> Listar();
        Task<Categorias?> BuscarPorId(int id);
        Task Atualizar(int id, Categorias categoria);
        
    }
}

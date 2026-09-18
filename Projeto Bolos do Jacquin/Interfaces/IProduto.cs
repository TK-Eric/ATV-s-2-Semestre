using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Interfaces
{
    public interface IProduto
    {
        Task Cadastrar(Produtos produto);

        Task Atualizar(int id, Produtos produto);

        Task<bool> Deletar(int id);

        Task<List<Produtos>> Listar(
            string? nome,
            int? categoria,
            decimal? precoMin,
            decimal? precoMax);

        Task<Produtos?> BuscarPorId(int id);
    }
}
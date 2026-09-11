using Microsoft.EntityFrameworkCore;
using Projeto_Bolos_do_Jacquin.BdContextBolos;
using Projeto_Bolos_do_Jacquin.Interfaces;
using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.Repositories
{
    public class UsuarioRepository : IUsuario
    {
        private readonly BolosContext _context;

        public UsuarioRepository(BolosContext context)
        {
            _context = context;
        }

        public async Task Cadastrar(Usuarios usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuarios?> BuscarPorId(Guid id)
        {
            return await _context.Usuarios
                .Include(p => p.Nome)
                .FirstOrDefaultAsync(p => p.IdUsuarios == id);
        }

        public async Task Atualizar(Guid id, Usuarios usuario)
        {
            var UsuarioBuscado = await _context.Produtos.FindAsync(id);

            if (UsuarioBuscado != null)
            {
                // Copia os novos valores para a entidade encontrada
                _context.Entry(UsuarioBuscado).CurrentValues.SetValues(usuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Deletar(Guid id)
        {
            var UsuarioBuscado = await _context.Usuarios.FindAsync(id);
            if (UsuarioBuscado != null)
            {
                _context.Usuarios.Remove(UsuarioBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Produtos>> Listar()
        {
            return await _context.Produtos
                .Include(c => c.IdProdutos)
                .Include(c => c.Nome)
                .Include(c => c.Descricao)
                .Include(c => c.DescricaoLonga)
                .Include(c => c.DescricaoCurta)
                .Include(c => c.Categoria)
                .Include(c => c.Avaliacoes)
                .Include(c => c.Imagem)
                .Include(c => c.Preco)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task Cadastrar(Usuarios usuario)
        {
            throw new NotImplementedException();
        }

        Task<List<Usuarios>> IUsuario.Listar()
        {
            throw new NotImplementedException();
        }

        public Task<Usuarios?> BuscarPorEmailESenha(string email, string senha)
        {
            throw new NotImplementedException();
        }

        Task<Usuarios?> IUsuario.BuscarPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Atualizar(Guid id, Usuarios usuario)
        {
            throw new NotImplementedException();
        }
    }
}

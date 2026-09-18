using Microsoft.EntityFrameworkCore;
using Projeto_Bolos_do_Jacquin.BdContextBolos;
using Projeto_Bolos_do_Jacquin.Interfaces;
using Projeto_Bolos_do_Jacquin.Models;
using Projeto_Bolos_do_Jacquin.Services;

namespace Projeto_Bolos_do_Jacquin.Repositories
{
    public class UsuarioRepository : IUsuario
    {
        private readonly BolosContext _context;

        public UsuarioRepository(BolosContext context)
        {
            _context = context;
        }

        public async Task<Usuarios?> BuscarPorId(int id)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u =>
                    u.IdUsuarios == id);
        }

        public async Task<List<Usuarios>> Listar()
        {
            return await _context.Usuarios
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> Deletar(int id)
        {
            var usuario =
                await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return false;
            }

            usuario.Situacao = false;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task Cadastrar(Usuarios usuario)
        {
            usuario.Senha =
                Cripitografia.GerarHash(usuario.Senha);

            await _context.Usuarios.AddAsync(usuario);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExiste(
            string email,
            int? idIgnorar = null)
        {
            email = email.Trim();

            return await _context.Usuarios
                .AnyAsync(u =>
                    u.Email == email &&
                    (!idIgnorar.HasValue ||
                     u.IdUsuarios != idIgnorar.Value));
        }

        public async Task<Usuarios?> BuscarPorEmailESenha(
            string email,
            string senha)
        {
            email = email.Trim();

            var usuario =
                await _context.Usuarios
                    .FirstOrDefaultAsync(u =>
                        u.Email == email);

            if (usuario == null)
            {
                return null;
            }

            bool senhaValida =
                Cripitografia.CompararHash(
                    senha,
                    usuario.Senha);

            if (!senhaValida)
            {
                return null;
            }

            return usuario;
        }

        public async Task Atualizar(
            int id,
            Usuarios usuario)
        {
            var usuarioBuscado =
                await _context.Usuarios.FindAsync(id);

            if (usuarioBuscado == null)
            {
                return;
            }

            usuarioBuscado.Nome =
                usuario.Nome;

            usuarioBuscado.Email =
                usuario.Email;

            if (!string.IsNullOrWhiteSpace(usuario.Senha))
            {
                usuarioBuscado.Senha =
                    Cripitografia.GerarHash(
                        usuario.Senha);
            }

            await _context.SaveChangesAsync();
        }
    }
}
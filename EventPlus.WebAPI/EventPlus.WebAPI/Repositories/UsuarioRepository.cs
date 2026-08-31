using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using EventPlus.WebAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repositories
{
    public class UsuarioRepository : IUsuario
    {
        private readonly EventContext _context;

        public UsuarioRepository(EventContext context)
        {
            _context = context;
        }

        public async Task Atualizar(Guid id, Usuario usuario)
        {
            var usuarioBuscado = await _context.Usuario.FindAsync(id);

            if (usuarioBuscado != null)
            {
                usuarioBuscado.Nome = usuario.Nome;
                usuarioBuscado.Email = usuario.Email;
                usuarioBuscado.IdTipoUsuario = usuario.IdTipoUsuario;

                if (!string.IsNullOrEmpty(usuario.Senha))
                {
                    usuarioBuscado.Senha = Criptografia.GerarHash(usuario.Senha);
                }

                _context.Usuario.Update(usuarioBuscado);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<Usuario?> BuscarPorEmailESenha(string email, string senha)
        {
            var usuario = await _context.Usuario
                .Include(u => u.IdTipoUsuarioNavigation)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                return null;
            }

            // Verifica se a senha digitada corresponde ao hash salvo no banco
            bool senhaValida = Criptografia.CompararHash(senha, usuario.Senha);

            if (!senhaValida) //!: operador de Negação 
            {
                return null;
            }

            return usuario;
        }

        public async Task<Usuario?> BuscarPorId(Guid id)
        {
            return await _context.Usuario
                .Include(u => u.IdTipoUsuarioNavigation)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        public async Task Cadastrar(Usuario usuario)
        {
            //Criptografamos a senha antes de salvar no banco
            usuario.Senha = Criptografia.GerarHash(usuario.Senha);

            await _context.Usuario.AddAsync(usuario);

            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var usuarioBuscado = await _context.Usuario.FindAsync(id);

            if (usuarioBuscado != null)
            {
                _context.Usuario.Remove(usuarioBuscado);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Usuario>> Listar()
        {
            //Alternativa,mas só retorna o id do tipo usuário -> return await _context.Usuario.AsNoTracking().ToListAsync();

            return await _context.Usuario
                .Include(u => u.IdTipoUsuarioNavigation)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
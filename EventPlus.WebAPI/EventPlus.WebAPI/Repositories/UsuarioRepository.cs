// Importa a classe EventContext, que é a configuração de conexão do seu banco de dados.
using EventPlus.WebAPI.BdContextEvent;

// Importa os "contratos" da aplicação, neste caso a interface IUsuario.
using EventPlus.WebAPI.Interfaces;

// Importa as classes que representam as tabelas do banco de dados (ex: a classe Usuario).
using EventPlus.WebAPI.Models;

// Importa ferramentas auxiliares (utilitários), como a classe Criptografia usada nas senhas.
using EventPlus.WebAPI.Utils;

// Importa a biblioteca do Entity Framework Core (o ORM responsável por traduzir C# para comandos SQL).
using Microsoft.EntityFrameworkCore;

// 'namespace' é o "endereço lógico" do arquivo, organizando-o dentro da pasta (camada) de Repositórios.
namespace EventPlus.WebAPI.Repositories
{
    // Declara a classe pública UsuarioRepository. 
    // Os dois pontos (: IUsuario) indicam que esta classe assinou um contrato e é OBRIGADA a implementar os métodos de IUsuario.
    public class UsuarioRepository : IUsuario
    {
        // Cria uma variável privada e somente leitura (readonly) para guardar a conexão com o banco.
        // O underline '_' é a convenção do C# para sinalizar que é uma variável exclusiva (privada) desta classe.
        private readonly EventContext _context;

        // ESTE É O CONSTRUTOR:
        // Toda vez que a API for usar este Repositório, o ASP.NET vai "injetar" automaticamente o contexto do banco aqui.
        public UsuarioRepository(EventContext context)
        {
            // Pegamos o contexto recebido e guardamos na nossa variável global '_context' para que todos os métodos abaixo possam acessá-lo.
            _context = context;
        }

        // Método assíncrono (Task) para cadastrar um novo usuário sem travar/congelar a API.
        public async Task Cadastrar(Usuario usuario)
        {
            // Pega a senha em texto limpo digitada pelo cliente, transforma num código embaralhado (Hash) e guarda de volta no objeto.
            usuario.Senha = Criptografia.GerarHash(usuario.Senha);

            // AddAsync avisa ao Entity Framework que queremos criar um registro (engatilha a ação na memória).
            await _context.Usuario.AddAsync(usuario);

            // O SaveChangesAsync consolida as mudanças, executando o comando "INSERT INTO" de fato lá no SQL Server.
            await _context.SaveChangesAsync();
        }

        // Retorna uma Lista de Usuários de forma assíncrona.
        public async Task<List<Usuario>> Listar()
        {
            // AsNoTracking(): Tira o monitoramento do Entity Framework (só vamos ler os dados, não alterar), deixando a consulta mais rápida.
            // ToListAsync(): Pega a tabela no banco e converte em uma Lista C#.
            return await _context.Usuario
                .Include(u => u.IdTipoUsuarioNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        // Método para atualizar um registro que já existe usando o ID dele.
        public async Task Atualizar(Guid id, Usuario usuario)
        {
            // FindAsync faz uma busca otimizada diretamente pela Chave Primária (ID) para localizar o registro antigo no banco.
            var usuarioBuscado = await _context.Usuario.FindAsync(id);

            // Verifica se o usuário realmente foi encontrado no banco antes de alterar algo (evita erros caso o ID não exista).
            if (usuarioBuscado != null)
            {
                // Substitui os dados antigos vindos do banco pelos dados novos recebidos do cliente (Insomnia).
                usuarioBuscado.Nome = usuario.Nome;
                usuarioBuscado.Email = usuario.Email;
                usuarioBuscado.IdTipoUsuario = usuario.IdTipoUsuario;

                // string.IsNullOrEmpty checa se a nova senha foi preenchida. 
                // A exclamação '!' inverte a lógica: "Se a senha NÃO for nula ou vazia..."
                if (!string.IsNullOrEmpty(usuario.Senha))
                {
                    // Gera o Hash da nova senha e substitui.
                    usuarioBuscado.Senha = Criptografia.GerarHash(usuario.Senha);
                }

                // Update sinaliza na memória que as propriedades desse objeto mudaram.
                _context.Usuario.Update(usuarioBuscado);

                // Dispara o comando "UPDATE" lá no banco de dados.
                await _context.SaveChangesAsync();
            }
        }

        // Método para excluir um usuário usando o ID.
        public async Task Deletar(Guid id)
        {
            // Localiza o usuário alvo pelo ID.
            var usuarioBuscado = await _context.Usuario.FindAsync(id);

            // Se achou o usuário...
            if (usuarioBuscado != null)
            {
                // Remove sinaliza na memória que esse objeto deve ser deletado.
                _context.Usuario.Remove(usuarioBuscado);

                // Dispara o comando "DELETE" no banco de dados.
                await _context.SaveChangesAsync();
            }
        }

        // Método que busca apenas um usuário pelo ID. A interrogação (Usuario?) significa que ele pode retornar nulo caso não encontre.
        public async Task<Usuario?> BuscarPorId(Guid id)
        {
            // FirstOrDefaultAsync varre os registros e devolve o primeiro que obedecer à regra: (o ID do usuário tem que ser igual ao 'id' do parâmetro).
            return await _context.Usuario.FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        // Método responsável pela Autenticação/Login do usuário.
        public async Task<Usuario?> BuscarPorEmailESenha(string email, string senha)
        {
            // PASSO 1: Vai no banco e busca o usuário que possui o e-mail digitado (sem checar a senha ainda).
            var usuario = await _context.Usuario
                .Include(u => u.IdTipoUsuarioNavigation)
                .FirstOrDefaultAsync(u => u.Email == email);

            // PASSO 2: O operador '&&' (E) obriga duas coisas a serem verdadeiras:
            // 1ª: O usuário deve existir (ser diferente de nulo).
            // 2ª: A senha limpa digitada tem que bater com o Hash estranho salvo no banco (método CompararHash).
            if (usuario == null)
            {
                // Se o email existir e a senha estiver certa, devolve o registro do usuário para a API gerar um Token de acesso.
                return null;
            }

            // Se cair aqui, é porque o e-mail não existe ou a senha está errada. Retorna nulo e nega o login.
            bool senhaValida = Criptografia.CompararHash(senha, usuario.Senha);
            if (!senhaValida) 
            {
                return null;
            }

            return usuario;
        }
    }
}
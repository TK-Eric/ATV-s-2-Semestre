using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Repositories
{
    // Essa classe é quem realmente "põe a mão na massa": ela implementa
    // a interface IEvent (o contrato que vimos antes), ou seja, aqui
    // fica o código de verdade que mexe no banco de dados
    public class EventRepository : IEvent
    {
        // O EventContext é a "porta de entrada" pro banco de dados
        // (é ele que o Entity Framework usa por trás dos panos
        // pra fazer o SELECT, INSERT, UPDATE, DELETE, etc)
        private readonly EventContext _context;

        // Recebe o EventContext prontinho via injeção de dependência
        public EventRepository(EventContext context)
        {
            _context = context;
        }

        // Ainda não foi implementado — só existe o "molde" do método.
        // Se alguém chamar esse método agora, vai dar erro em tempo de
        // execução (NotImplementedException)
        public Task Atualizar(Guid id, Evento evento)
        {
            throw new NotImplementedException();
        }

        // Também ainda não foi feito
        public Task<Evento?> BuscarPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        // Esse aqui já está pronto: cadastra um evento no banco
        public async Task Cadastrar(Evento evento)
        {
            // Adiciona o evento na memória do EF Core (ainda não salvou
            // no banco, só "avisou" que isso precisa ser inserido)
            await _context.Evento.AddAsync(evento);

            // Agora sim: manda de verdade pro banco (executa o INSERT)
            await _context.SaveChangesAsync();
        }

        // Ainda não implementado
        public Task Deletar(Guid id)
        {
            throw new NotImplementedException();
        }

        // Ainda não implementado
        public Task<List<Evento>> Listar()
        {
            throw new NotImplementedException();
        }

        // Ainda não implementado
        public Task<List<Evento>> ListarProximos()
        {
            throw new NotImplementedException();
        }
    }
}
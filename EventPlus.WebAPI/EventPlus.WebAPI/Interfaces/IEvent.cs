using EventPlus.WebAPI.Models;

namespace EventPlus.WebAPI.Interfaces
{
    // Isso aqui é uma "interface": uma lista de promessas do que a classe
    // que for usar ela (tipo um EventRepository ou EventService) precisa
    // saber fazer. A interface não faz nada sozinha, só define "o que tem que existir"
    public interface IEvent
    {
        // Promete um jeito de cadastrar (salvar) um novo evento
        Task Cadastrar(Evento evento);

        // Promete um jeito de atualizar um evento já existente,
        // usando o Id dele pra saber qual é
        Task Atualizar(Guid id, Evento evento);

        // Promete um jeito de deletar um evento, usando o Id dele
        Task Deletar(Guid id);

        // Promete um jeito de listar todos os eventos cadastrados
        Task<List<Evento>> Listar();

        // Promete um jeito de listar só os eventos que ainda vão acontecer
        // (os "próximos", provavelmente filtrando por data)
        Task<List<Evento>> ListarProximos();

        // Promete um jeito de buscar um evento específico pelo Id
        // O "?" em "Evento?" significa que pode não achar nada e retornar nulo
        Task<Evento?> BuscarPorId(Guid id);
    }
}
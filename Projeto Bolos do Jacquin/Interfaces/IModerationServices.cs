namespace Projeto_Bolos_do_Jacquin.WebAPI.Interfaces
{
    public interface IModerationService
    {

        Task<bool> ModerarTexto(string texto);
    }
}

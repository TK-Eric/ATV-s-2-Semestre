namespace EventPlus.WebAPI.Interfaces
{
    public interface IModerationService
    {
        Task<bool> ModeratorTexto(string texto);
    }
}

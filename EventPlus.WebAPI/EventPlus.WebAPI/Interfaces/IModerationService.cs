namespace EventPlus.WebAPI.Interfaces
{
    public interface IModerationService
    {
        
        Task<bool> ModerarTexto(string texto);
    }
}

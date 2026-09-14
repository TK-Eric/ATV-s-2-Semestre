namespace Projeto_Bolos_do_Jacquin.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImagem(IFormFile arquivo);
    }
}

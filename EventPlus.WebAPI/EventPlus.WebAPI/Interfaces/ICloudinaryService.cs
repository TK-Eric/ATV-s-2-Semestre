namespace EventPlus.WebAPI.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImagem(IFormFile arquivo);
    }
}

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Utils;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace EventPlus.WebAPI.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var credenciais = options.Value;

            var account = new Account(credenciais.CloudName, credenciais.ApiKey, credenciais.ApiSecret);

            _cloudinary = new Cloudinary(account);

            _cloudinary.Api.Secure = true;
        }

        public async Task<string> UploadImagem(IFormFile arquivo)
        {
            //abre um fluxo de leitura do arquivo 
            //using: garnte que o stream sera fechado apos o uso
            using var stream = arquivo.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                //o arquivo em sim, o nome originl mais o fluxo de bytes a enviar
                File = new FileDescription(arquivo.FileName, stream),
                //pasta de destino dentro do cloudinary
                Folder = "eventplus/eventos"
            };
            //envia a imagem para o cloudnary e aguarda a resposta com os dados do upload
            var resultado = await _cloudinary.UploadAsync(uploadParams);

            //retorna so o que interessa para a aplicação(URL segura da imagem)
            //que depois sera salva no campo imagem/URL
            return resultado.SecureUrl.AbsoluteUri;
        }
    }
}
    
namespace EventPlus.WebAPI.Utils
{
    public class CloudinarySettings
    {
        //nome da conta no cloudinay
        public string CloudName { get; set; } = string.Empty;

        //chave publica de identificação da API

        public string ApiKey { get; set; } = string.Empty;

        // Chave secreta que assina/autentica as requisições

        public string ApiSecret { get; set; } = string.Empty;


    }
}

using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Utils;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EventPlus.WebAPI.Services
{
    public class SightengineModerationService : IModerationService
    {
        private readonly HttpClient _http;
        private readonly string _apiUser;
        private readonly string _apiSecret;

        //acima desse limiar, a categoria é considerada violação
        private const double Limiar = 0.5;

        public SightengineModerationService(HttpClient http, IOptions<SightengineSettings> options)
        {
            _http = http;
            _apiUser = options.Value.ApiUser;
            _apiSecret = options.Value.ApiSecret;
        }


        public async Task<bool> ModerarTexto(string texto)
        {
            //FormUrlEncodedContent: definir que as credenciais vão no corpo da requisição
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["text"] = texto,
                ["lang"] = "pt",
                ["mode"] = "ml",
                ["api_user"] = _apiUser,
                ["api_secret"] = _apiSecret
            });

            // "text/check.json" : endpoint da api externa
            // form: dados que serão enviados junto a requisição(texto a ser moderado etc..
            var resposta = await _http.PostAsync("text/check.json", form);

            // verifica se a resposta(http post) foi bem sucedida
            // se o status for um erro, lança uma exception
            resposta.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(
                await resposta.Content.ReadAsStringAsync()
            );

            // obtém o elemento raiz do json
            // acesso as propriedades do json(array e etc..)
            var root = doc.RootElement;

            // obtém a propriedade status do json e verifica se o valor é diferente de sucess
            if (root.GetProperty("status").GetString() != "success")
            {
                // tenta obter a prop "error", e dentro dela a mensagem de erro
                // caso não não retorne, utilizamos um mensagem de erro desconhecido
                var msg = root.TryGetProperty("error", out var err) && err.TryGetProperty("message", out var m) ? m.GetString() : "erro desconhecido";

                throw new Exception($"Sightengine: {msg}");
            }

            var classes = root.GetProperty("moderation_classes");

            foreach (var prop in classes.EnumerateObject())
            {
                if (prop.Name == "available") continue;
                if (prop.Value.ValueKind == JsonValueKind.Number && prop.Value.GetDouble() >= Limiar)
                    return true; // reprovado passou do limiar
            }

            return false;//aprovado : não passou do limiar
        }
    }
}

// Pendências :

// 3) Cadastrar uma presença p/ o usuário (Cadastrar)
// 4) Cadastrar um comentário (Cadastrar)
// 5) Terminar todos os CRUDSSSSSSSSSSSSSSSSS!!!!!!
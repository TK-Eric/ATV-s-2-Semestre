using EventPlus.WebAPI.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EventPlus.WebAPI.Services
{
    public class SightengineModerationService : IModerationService
    {
        private readonly HttpClient _http;
        private readonly string _apiUser;
        private readonly string _apiSecret;

        private const double Limiar = 0.5;

        public SightengineModerationService(HttpClient http, IOptions<SightengineSettings> options)
        {
            _http = http;
            _apiUser = options.Value.ApiUser;
            _apiSecret = options.Value.ApiSecret;
        }

        public async Task<bool> ModerarTexto(string texto)
        {
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["text"] = texto,
                ["lang"] = "pt",
                ["mode"] = "ml",
                ["api_user"] = _apiUser,
                ["api_secret"] = _apiSecret
            });

            var resposta = await _http.PostAsync("text/check.json", form);
            resposta.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(
                await resposta.Content.ReadAsStringAsync()
            );

            var root = doc.RootElement;

            if (root.GetProperty("status").GetString() != "success")
            {
                var msg = root.TryGetProperty("error", out var err) && err.TryGetProperty("message", out var m)
                    ? m.GetString()
                    : "erro desconhecido";

                throw new Exception($"Sightengine: {msg}");
            }

            var classes = root.GetProperty("moderation_classes");

            foreach (var prop in classes.EnumerateObject())
            {
                if (prop.Name == "available") continue;
                if (prop.Value.ValueKind == JsonValueKind.Number && prop.Value.GetDouble() >= Limiar)
                    return true;
            }

            return false;
        }
    }
}

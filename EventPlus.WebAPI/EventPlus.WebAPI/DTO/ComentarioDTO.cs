using EventPlus.WebAPI.Utils;
using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class ComentarioDTO
    {
        [Required(ErrorMessage = "O texto do comentário é obrigatório.")]
        [StringLength(200, ErrorMessage = "O comentário deve ter no máximo 200 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O identificador do evento é obrigatório.")]
        public Guid IdEvento { get; set; }
        public Guid? IdUsuario { get; internal set; }


        // --- Sightengine (plano Free, sem cartão) ---
        builder.Services.Configure<SightengineSettings>(builder.Configuration.GetSection("Sightengine"));

builder.Services.AddHttpClient<IModerationService, SightengineModerationService>(client =>
{
    client.BaseAddress = new Uri("https://api.sightengine.com/1.0/");
    });

    public Guid? IdUsuario { get; set; }
}
}

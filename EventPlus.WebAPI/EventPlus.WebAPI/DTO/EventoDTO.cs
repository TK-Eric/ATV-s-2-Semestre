using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class EventoDTO
    {
        [Required(ErrorMessage = "O nome do evento é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
        public string NomeEvento { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição do evento é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data do evento é obrigatória.")]
        public DateTime DataEvento { get; set; }

        public string? ImagemUrl { get; set; }

        public IFormFile? ArquivoImagem { get; set; }

        public Guid? IdTipoEvento { get; set; }

        public Guid? IdInstituicao { get; set; }
    }
}
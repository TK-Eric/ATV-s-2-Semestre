using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class TipoEventoDTO
    {
        [Required(ErrorMessage = "O título do tipo de evento é obrigatório!")]
        [StringLength(100, ErrorMessage = "O Título pode ter no máximo 100 caracteres")]
        public string TituloTipoEvento { get; set; } = string.Empty;
    }
}
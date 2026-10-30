using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class TipoEventoDTO
    {
        [StringLength(100, ErrorMessage = "O Título pode ter no máximo 100 caracteres")]
        public string? TituloTipoEvento { get; set; }

        [StringLength(100, ErrorMessage = "O Título pode ter no máximo 100 caracteres")]
        public string? Titulo { get; set; }
    }
}

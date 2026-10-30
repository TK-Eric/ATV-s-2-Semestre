using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class TipoUsuarioDTO
    {
        [StringLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
        public string? Titulo { get; set; }

        [StringLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
        public string? TituloTipoUsuario { get; set; }
    }
}

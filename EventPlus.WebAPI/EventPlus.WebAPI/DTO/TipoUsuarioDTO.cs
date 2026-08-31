using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class TipoUsuarioDTO
    {
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;
    }
}
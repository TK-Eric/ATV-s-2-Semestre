using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class TipoEventoDTO
    {
        /// <summary>
        /// Titulo do tipo usuario
        /// </summary>
        [Required(ErrorMessage = "O titulo é obrigatorio. ")]
        [StringLength(100, ErrorMessage = "o titulo pode ter no maximo 100 caracteres.")]
        public string TituloTipoEvento { get; set; } = string.Empty;
    }
}

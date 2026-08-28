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

        [Required(ErrorMessage = "A data é obrigatória")]
        public DateTime DataEvento { get; set; }

        [Required(ErrorMessage = "A Descrição do evento é obrigatória")]
        public string Descricao { get; set; } = string.Empty;

        public IFormFile? ImagemUrl { get; set; }

        public Guid? IdTipoEvento { get; set; }

        public Guid? IdInstituicao { get; set; }
    }


}


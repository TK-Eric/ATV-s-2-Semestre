using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class PresencaDTO
    {
        public bool Situacao { get; set; }

        [Required(ErrorMessage = "O identificador do evento é obrigatório.")]
        public Guid IdEvento { get; set; }

        public Guid? IdUsuario { get; set; }
    }
}
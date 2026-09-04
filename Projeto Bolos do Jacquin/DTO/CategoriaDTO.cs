using System.ComponentModel.DataAnnotations;

namespace Projeto_Bolos_do_Jacquin.DTO
{
    public class CategoriaDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
        public string Nome { get; set; }
        public Guid? IdCategoria { get; set; }

    }
}

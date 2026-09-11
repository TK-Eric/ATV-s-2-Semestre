using System.ComponentModel.DataAnnotations;

namespace Projeto_Bolos_do_Jacquin.DTO
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome pode ter no máximo 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        [StringLength(150, ErrorMessage = "O email pode ter no máximo 150 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(255, ErrorMessage = "A senha pode ter no máximo 255 caracteres.")]
        public string Senha { get; set; } = string.Empty;

        public string Perfil { get; set; } = string.Empty;

        public bool? Situacao { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Projeto_Bolos_do_Jacquin.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "O e-mail é obrigatorio para autenticação!")]
        [EmailAddress(ErrorMessage = "informe um email valido")]

        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha é obrigatorio para autenticação!")]
        [StringLength(60, MinimumLength = 8, ErrorMessage = "informe um email valido.")]


        public string Senha { get; set; } = string.Empty;

    }
}

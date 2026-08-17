using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage ="Campo Obrigatorio")]
        [StringLength(100, ErrorMessage ="o Nome deve ter no maximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "Campo Obrigatorio")]
        [EmailAddress(ErrorMessage ="informe um email valido!")]

        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Campo Obrigatorio")]
        [StringLength(60, MinimumLength = 8,  ErrorMessage = "Senha errada")]


        public string Senha { get; set; } = string.Empty;
        [Required(ErrorMessage = "Campo Obrigatorio")]


        public Guid? IdTipoUsuario { get; set; }


    }
}

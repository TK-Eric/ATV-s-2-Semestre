using System.ComponentModel.DataAnnotations;

namespace EventPlus.WebAPI.DTO
{
    public class InstituicaoDTO
    {

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter 14 caracteres.")]
        public string CNPJ { get; set; } = string.Empty;


        [Required(ErrorMessage = "O Nome Fantasia é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome Fantasia pode ter no máximo 100 caracteres.")]
        public string NomeFantasia { get; set; } = string.Empty;


        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(100, ErrorMessage = "O endereço pode ter no máximo 100 caracteres.")]
        public string Endereco { get; set; } = string.Empty;
    }
}
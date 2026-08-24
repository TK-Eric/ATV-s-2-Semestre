using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventPlus.WebAPI.DTO
{
    public class InstituicaoDTO
    {
        [Required(ErrorMessage = "O Id da instituição é obrigatorio para autenticação!")]
        [EmailAddress(ErrorMessage = "informe um Id da instituição valido")]

        public string IdInstituicao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Cnpj é obrigatorio para autenticação!")]
        [StringLength(14, ErrorMessage = "informe um Cnpj valido.")]


        public string Cnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "O NomeFantasia é obrigatorio para autenticação!")]
        [StringLength(100, ErrorMessage = "informe um NomeFantasia valido.")]


        public string NomeFantasia { get; set; } = string.Empty;


        [Required(ErrorMessage = "O Endereco é obrigatorio para autenticação!")]
        [StringLength(100, ErrorMessage = "informe um Endereco valido.")]


        public string Endereco { get; set; } = string.Empty;

    }
}

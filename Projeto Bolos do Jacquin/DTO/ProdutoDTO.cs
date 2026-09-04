using System.ComponentModel.DataAnnotations;

namespace Projeto_Bolos_do_Jacquin.DTO
{
    public class ProdutoDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição do evento é obrigatória.")]
        public string DescricaoCurta { get; set; } = string.Empty;

        public string DescricaoLonga { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data do evento é obrigatória.")]
        public DateTime DataEvento { get; set; }

        public string? EndereçoImagem { get; set; }

        public IFormFile? ArquivoImagem { get; set; }

        public Guid? IdProduto { get; set; }

        public Guid? IdCategoria { get; set; }

        public bool Situacao { get; set; }

        public bool Disponibilidade { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Projeto_Bolos_do_Jacquin.DTO
{
    public class ProdutoDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(
            150,
            ErrorMessage = "O nome pode ter no máximo 150 caracteres."
        )]
        public string Nome { get; set; } = string.Empty;


        [Required(ErrorMessage = "A descrição curta é obrigatória.")]
        public string DescricaoCurta { get; set; } = string.Empty;


        [Required(ErrorMessage = "A descrição longa é obrigatória.")]
        public string DescricaoLonga { get; set; } = string.Empty;


        [Required(ErrorMessage = "O endereço da imagem é obrigatório.")]
        [StringLength(
            500,
            ErrorMessage = "O endereço da imagem pode ter no máximo 500 caracteres."
        )]
        [Url(ErrorMessage = "O endereço da imagem deve ser uma URL válida.")]
        public string EnderecoImagem { get; set; } = string.Empty;


        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "A categoria informada é inválida."
        )]
        public int IdCategoria { get; set; }


        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(
            0.01,
            double.MaxValue,
            ErrorMessage = "O preço deve ser maior que zero."
        )]
        public decimal Preco { get; set; }


        public bool Situacao { get; set; }


        public bool Disponibilidade { get; set; }
    }
}
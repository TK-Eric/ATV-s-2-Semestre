using System.ComponentModel.DataAnnotations;

namespace Projeto_Bolos_do_Jacquin.DTO
{
    public class AvaliacaoDTO
    {
        public Guid? IdProdutos { get; set; }
        public int? Nota { get; set; }
        public string? Comentario { get; set; }
        public DateTime DataCriacao { get; set; }
       
    }
}

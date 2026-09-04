using System.ComponentModel.DataAnnotations;

namespace Projeto_Bolos_do_Jacquin.DTO
{
    public class AvaliacaoDTO
    {
        public Guid? IdUsuario { get; set; }
        public Guid? IdAvaliacao { get; set; }
        public Guid? IdProduto { get; set; }
        public int? Nota { get; set; }
        public string? Comentario { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }


    }
}

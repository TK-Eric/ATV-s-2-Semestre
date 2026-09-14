using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Projeto_Bolos_do_Jacquin.Models;

public partial class Produtos
{
    [Key]
    public int IdProdutos { get; set; }

    [Required]
    [StringLength(150)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    public decimal Preco { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? EnderecoImagem { get; set; }

    public string? DescricaoCurta { get; set; }

    public string? DescricaoLonga { get; set; }

    public bool Disponibilidade { get; set; }

    public bool Situacao { get; set; }

    [Required]
    public int IdCategoria { get; set; }

    [InverseProperty("Produto")]
    public virtual ICollection<Avaliacoes> Avaliacoes { get; set; }
        = new List<Avaliacoes>();

    [ForeignKey(nameof(IdCategoria))]
    [InverseProperty("Produtos")]
    public virtual Categorias Categoria { get; set; } = null!;
}
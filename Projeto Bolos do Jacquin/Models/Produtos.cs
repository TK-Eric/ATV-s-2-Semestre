using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Projeto_Bolos_do_Jacquin.Models;

public partial class Produtos
{
    [Key]
    public int IdProdutos { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Preco { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Imagem { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Descricao { get; set; }

    public bool? Disponibilidade { get; set; }

    public int CategoriaId { get; set; }

    [InverseProperty("Produto")]
    public virtual ICollection<Avaliacoes> Avaliacoes { get; set; } = new List<Avaliacoes>();

    [ForeignKey("CategoriaId")]
    [InverseProperty("Produtos")]
    public virtual Categorias Categoria { get; set; } = null!;
}

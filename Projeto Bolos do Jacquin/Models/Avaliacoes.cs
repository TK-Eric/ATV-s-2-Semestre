using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Projeto_Bolos_do_Jacquin.Models;

public partial class Avaliacoes
{
    [Key]
    public int IdAvaliacoes { get; set; }

    public int Nota { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? Comentario { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DataCriacao { get; set; }

    public int UsuarioId { get; set; }

    public int ProdutoId { get; set; }

    [ForeignKey("ProdutoId")]
    [InverseProperty("Avaliacoes")]
    public virtual Produtos Produto { get; set; } = null!;

    [ForeignKey("UsuarioId")]
    [InverseProperty("Avaliacoes")]
    public virtual Usuarios Usuario { get; set; } = null!;
}

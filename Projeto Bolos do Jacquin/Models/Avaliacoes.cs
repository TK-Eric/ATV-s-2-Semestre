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
    public string? Comentario { get; set; }
    public DateTime DataCriacao { get; set; }

    public int IdUsuarios { get; set; }
    public int? IdProdutos { get; set; }

    [ForeignKey("IdProdutos")]
    [InverseProperty("Avaliacoes")]
    public virtual Produtos? Produto { get; set; }

    [ForeignKey("IdUsuarios")]
    [InverseProperty("Avaliacoes")]
    public virtual Usuarios Usuario { get; set; } = null!;

    public bool Exibe { get; set; }
}

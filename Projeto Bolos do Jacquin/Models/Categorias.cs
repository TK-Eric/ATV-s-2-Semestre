using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace Projeto_Bolos_do_Jacquin.Models;

[Index("Nome", Name = "UQ__Categori__7D8FE3B26E205219", IsUnique = true)]
public partial class Categorias
{
    [Key]
    public Guid? IdCategorias { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string NomeCategoria { get; set; } = null!;

    [InverseProperty("Categoria")]
    public virtual ICollection<Produtos> Produtos { get; set; } = new List<Produtos>();
}

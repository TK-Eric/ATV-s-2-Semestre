using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Projeto_Bolos_do_Jacquin.Models;

[Index("Email", Name = "UQ__Usuarios__A9D105345D5C7F82", IsUnique = true)]
public partial class Usuarios
{
    [Key]
    public int IdUsuarios { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Senha { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Perfil { get; set; } = null!;

    public bool? Situacao { get; set; }

    [InverseProperty("Usuario")]
    public virtual ICollection<Avaliacoes> Avaliacoes { get; set; } = new List<Avaliacoes>();
}

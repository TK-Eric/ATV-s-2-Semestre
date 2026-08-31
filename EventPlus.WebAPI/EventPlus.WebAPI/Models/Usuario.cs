using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

[Index("Email", Name = "UQ__Usuario__A9D1053408B16312", IsUnique = true)]
public partial class Usuario
{
    [Key]
    public Guid IdUsuario { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nome { get; set; } = null!;

    [StringLength(256)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(60)]
    [Unicode(false)]
    [JsonIgnore]//nunca serializa a senha na reposta
    public string Senha { get; set; } = null!;

    public Guid? IdTipoUsuario { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Comentario> Comentario { get; set; } = new List<Comentario>();

    [ForeignKey("IdTipoUsuario")]
    [InverseProperty("Usuario")]
    public virtual TipoUsuario? IdTipoUsuarioNavigation { get; set; }

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Presenca> Presenca { get; set; } = new List<Presenca>();
}
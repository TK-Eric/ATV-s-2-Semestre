using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Models;

// Essa classe representa a tabela "Evento" no banco de dados.
// Cada propriedade aqui vira uma coluna na tabela.
// (Provavelmente foi gerada automaticamente pelo Entity Framework
// a partir do banco já existente — "scaffolding")
public partial class Evento
{
    // [Key] marca essa propriedade como a chave primária (PK) da tabela,
    // ou seja, o identificador único de cada evento
    [Key]
    public Guid IdEvento { get; set; }

    // Chave estrangeira (FK) pra tabela TipoEvento.
    // O "?" (Guid?) significa que pode ficar em branco (nulo),
    // ou seja, um evento pode não ter tipo definido
    public Guid? IdTipoEvento { get; set; }

    // Chave estrangeira (FK) pra tabela Instituicao.
    // Também pode ficar nula
    public Guid? IdInstituicao { get; set; }

    // Nome do evento. Limite de 100 caracteres no banco.
    // [Unicode(false)] indica que o banco guarda como texto comum
    // (não precisa de acentuação/caracteres especiais tipo emoji)
    // "= null!" é só pra avisar o compilador "confia, isso nunca vai ser nulo de verdade"
    [StringLength(100)]
    [Unicode(false)]
    public string NomeEvento { get; set; } = null!;

    // Descrição do evento. [Column(TypeName = "text")] diz que no banco
    // essa coluna é do tipo "text" (aceita textos longos, sem limite fixo)
    [Column(TypeName = "text")]
    public string Descricao { get; set; } = null!;

    // Data (e hora) em que o evento vai acontecer.
    // [Column(TypeName = "datetime")] garante que no banco fica salvo
    // como "datetime" mesmo
    [Column(TypeName = "datetime")]
    public DateTime DataEvento { get; set; }

    // Link da imagem do evento (a URL que vem do Cloudinary, lembra do
    // controller que vimos antes?).
    // [Column("imagemURL")] diz que no banco o nome da coluna é "imagemURL"
    // (diferente do nome da propriedade "ImagemUrl" no C#)
    // O "?" (string?) significa que é opcional, pode não ter imagem
    [Column("imagemURL")]
    [StringLength(200)]
    [Unicode(false)]
    public string? ImagemUrl { get; set; }

    // Lista de comentários que esse evento tem.
    // Isso é um relacionamento "1 pra muitos": um evento pode ter
    // vários comentários. O EF Core usa isso pra saber como
    // buscar/juntar (join) essas informações quando precisar
    [InverseProperty("IdEventoNavigation")]
    public virtual ICollection<Comentario> Comentario { get; set; } = new List<Comentario>();

    // Essa propriedade dá acesso direto ao objeto Instituicao relacionado
    // (não só o Id, mas a instituição completa, se você pedir pro EF carregar)
    [ForeignKey("IdInstituicao")]
    [InverseProperty("Evento")]
    public virtual Instituicao? IdInstituicaoNavigation { get; set; }

    // Mesma ideia, mas pro TipoEvento relacionado
    [ForeignKey("IdTipoEvento")]
    [InverseProperty("Evento")]
    public virtual TipoEvento? IdTipoEventoNavigation { get; set; }

    // Lista de presenças confirmadas nesse evento.
    // Outro relacionamento "1 pra muitos": um evento pode ter
    // várias pessoas confirmando presença
    [InverseProperty("IdEventoNavigation")]
    public virtual ICollection<Presenca> Presenca { get; set; } = new List<Presenca>();
}
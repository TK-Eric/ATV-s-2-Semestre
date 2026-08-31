using System;
using System.Collections.Generic;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.BdContextEvent;

public partial class EventContext : DbContext
{
    public EventContext()
    {
    }

    public EventContext(DbContextOptions<EventContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comentario> Comentario { get; set; }

    public virtual DbSet<Evento> Evento { get; set; }

    public virtual DbSet<Instituicao> Instituicao { get; set; }

    public virtual DbSet<Presenca> Presenca { get; set; }

    public virtual DbSet<TipoEvento> TipoEvento { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.IdComentario).HasName("PK__Comentar__DDBEFBF9A275BBD1");

            entity.Property(e => e.IdComentario).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.Comentario).HasConstraintName("FK__Comentari__IdEve__6383C8BA");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Comentario)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Comentari__IdUsu__6477ECF3");
        });

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(e => e.IdEvento).HasName("PK__Evento__034EFC046319B558");

            entity.Property(e => e.IdEvento).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdInstituicaoNavigation).WithMany(p => p.Evento)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Evento__IdInstit__5AEE82B9");

            entity.HasOne(d => d.IdTipoEventoNavigation).WithMany(p => p.Evento)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Evento__IdTipoEv__59FA5E80");
        });

        modelBuilder.Entity<Instituicao>(entity =>
        {
            entity.HasKey(e => e.IdInstituicao).HasName("PK__Institui__B771C0D8FFC7983C");

            entity.Property(e => e.IdInstituicao).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Presenca>(entity =>
        {
            entity.HasKey(e => e.IdPresenca).HasName("PK__Presenca__50FB6F5D7A12364E");

            entity.Property(e => e.IdPresenca).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.Presenca).HasConstraintName("FK__Presenca__IdEven__5EBF139D");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Presenca)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Presenca__IdUsua__5FB337D6");
        });

        modelBuilder.Entity<TipoEvento>(entity =>
        {
            entity.HasKey(e => e.IdTipoEvento).HasName("PK__TipoEven__CDB3A3BEC16CE581");

            entity.Property(e => e.IdTipoEvento).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTipoUsuario).HasName("PK__TipoUsua__CA04062BC8D2CF9F");

            entity.Property(e => e.IdTipoUsuario).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97A4AB46FC");

            entity.Property(e => e.IdUsuario).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.IdTipoUsuarioNavigation).WithMany(p => p.Usuario)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Usuario__IdTipoU__5629CD9C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
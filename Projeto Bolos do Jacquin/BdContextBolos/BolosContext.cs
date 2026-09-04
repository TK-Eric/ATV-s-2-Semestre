using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Projeto_Bolos_do_Jacquin.Models;

namespace Projeto_Bolos_do_Jacquin.BdContextBolos;

public partial class BolosContext : DbContext
{
    public BolosContext()
    {
    }

    public BolosContext(DbContextOptions<BolosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Avaliacoes> Avaliacoes { get; set; }

    public virtual DbSet<Categorias> Categorias { get; set; }

    public virtual DbSet<Produtos> Produtos { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=D02S22-1252888\\MSSQLSERVER2;Database=BolosDoJacquin;User Id=sa;Password=Senai@134;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Avaliacoes>(entity =>
        {
            entity.HasKey(e => e.IdAvaliacoes).HasName("PK__Avaliaco__0CED7C87F3561074");

            entity.Property(e => e.DataCriacao).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Produto).WithMany(p => p.Avaliacoes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Avaliacoe__Produ__01142BA1");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Avaliacoes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Avaliacoe__Usuar__00200768");
        });

        modelBuilder.Entity<Categorias>(entity =>
        {
            entity.HasKey(e => e.IdCategorias).HasName("PK__Categori__0185FF077BA8EBA4");
        });

        modelBuilder.Entity<Produtos>(entity =>
        {
            entity.HasKey(e => e.IdProdutos).HasName("PK__Produtos__30029F98F3966CA7");

            entity.Property(e => e.Disponibilidade).HasDefaultValue(true);

            entity.HasOne(d => d.Categoria).WithMany(p => p.Produtos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Produtos__Catego__7C4F7684");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.IdUsuarios).HasName("PK__Usuarios__EAEBAC8FE7D5CB79");

            entity.Property(e => e.Situacao).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

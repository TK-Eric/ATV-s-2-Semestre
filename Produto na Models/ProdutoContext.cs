using Microsoft.EntityFrameworkCore;

public class ProdutoContext : DbContext
{
    public ProdutoContext(DbContextOptions<ProdutoContext> options)
        : base(options)
    {
    }

    public DbSet<Produto> Produtos { get; set; }
}
using Microsoft.EntityFrameworkCore;
using ProdutoCodeFirstAPI.Models;

namespace ProdutoCodeFirstAPI.Contexts
{
    public class ProdutoContext : DbContext
    {
        public ProdutoContext(DbContextOptions<ProdutoContext> options)
            : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Models
{
    public class AppDataContext : DbContext
    {
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Biblioteca.db");
        }
    }
}
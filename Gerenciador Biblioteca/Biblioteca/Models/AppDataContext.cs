using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Models
{
    public class AppDataContext : DbContext
    {
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Autor> Autores { get; set; }

        public DbSet<LivroAutor> LivrosAutores { get; set; }        
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Biblioteca.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LivroAutor>()
                .HasKey(x => new { x.LivroId, x.AutorId });
            modelBuilder.Entity<LivroAutor>()
                .HasOne(bc => bc.Livro)
                .WithMany(b => b.LivrosAutores)
                .HasForeignKey(bc => bc.LivroId);
            modelBuilder.Entity<LivroAutor>()
                .HasOne(bc => bc.Autor)
                .WithMany(c => c.LivrosAutores)
                .HasForeignKey(bc => bc.AutorId);
        }
    }
}
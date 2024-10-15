namespace Biblioteca.Models
{
    public class LivroAutor
    {
        public Guid AutorId { get; set; }
        public Autor? Autor { get; set; }
        public string? LivroId { get; set; }
        public Livro? Livro { get; set; }
    }
}
namespace Biblioteca.Models
{
    public class Autor
    {
        public Guid AutorId { get; set;}

        public string? Nome { get; set; }

        public string? Sobrenome { get; set; }

        public string? Pais { get; set;}

        public ICollection<LivroAutor> LivrosAutores { get; set; }

        public Autor()
        {
            LivrosAutores = new HashSet<LivroAutor>();
        }
    }
}
using System.Text.Json.Serialization; 

namespace Biblioteca.Models
{
    public class Livro
    {
        public Livro(){
            LivroId = Guid.NewGuid().ToString();
            CriadoEm = DateTime.Now;
            LivrosAutores = new HashSet<LivroAutor>();
        }

        public string LivroId { get; set; }

        public string? Titulo { get; set; }

        public string? Genero { get; set; }

        public int QtdExemplares { get; set; }

        public DateTime CriadoEm { get; set; }
        
        public int AnoLancamento { get; set; }

        public string? Editora { get; set; }

        public Autor Autor { get; set; }

        public int AutorId { get; set; }

        [JsonIgnore]
        public ICollection<LivroAutor> LivrosAutores { get; set; }
    }
}
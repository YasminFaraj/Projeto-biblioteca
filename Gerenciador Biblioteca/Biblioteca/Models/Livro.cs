namespace Biblioteca.Models
{
    public class Livro
    {
        public Livro(){
            Id = Guid.NewGuid().ToString();
            CriadoEm = DateTime.Now;
        }

        public string? Id { get; set; }
        public string? Titulo { get; set; }
        public string? Autor { get; set; }
        public string? Genero { get; set; }
        public int qtdExemplares { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
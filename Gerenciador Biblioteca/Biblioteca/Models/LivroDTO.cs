public class LivroDTO
{
    public string? Titulo { get; set; }
    public string? Genero { get; set; }
    public int QtdExemplares { get; set; }
    public int AnoLancamento { get; set; }
    public string? Editora { get; set; }
    public List<LivroAutorDTO> LivrosAutores { get; set; } = new List<LivroAutorDTO>(); //essa parte inicia a lista e nao aparece o warning de Non-nullable property
}

using System;
namespace Biblioteca.Models;

public class Categoria
{
    public int CategoriaId { get; set; }
    public string? Titulo { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.Now;
}
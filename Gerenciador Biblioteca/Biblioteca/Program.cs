using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
var app = builder.Build();

List<Autor> autores = new List<Autor>{
    new Autor { Nome = "Taylor J. R." },
    new Autor { Nome = "Emily Bronte" },
};

List<Livro> livros = new List<Livro>{
    new Livro {
        Titulo = "Os Sete Maridos de Evelyn Hugo",
        QtdExemplares = 3, 
        LivrosAutores = new List<LivroAutor>{
            new LivroAutor { Autor = autores[0] }
        }
    },
    new Livro {
        Titulo = "O Morro dos Ventos Uivantes", 
        LivrosAutores = new List<LivroAutor>{
            new LivroAutor { Autor = autores[1] }
        },
        QtdExemplares = 2},
};

app.MapGet("/", () => "API de Livros");

app.MapGet("/biblioteca/livro/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Livros.Any()){
        return Results.Ok(ctx.Livros.ToList());
    }    
    return Results.NotFound();
});

app.MapGet("/biblioteca/livro/buscar/{id}", ([FromRoute] string id, [FromServices] AppDataContext ctx) =>{
    Livro? livro = ctx.Livros.Find(id);
    if (livro == null){
        return Results.NotFound();
    } 
    return Results.Ok(livro);
});

app.MapPost("/biblioteca/livro/cadastrar", ([FromBody] Livro livro, [FromServices] AppDataContext ctx) =>
{
    foreach (var livroAutor in livro.LivrosAutores)
    {
        if (livroAutor.Autor != null) 
        {
            var autorExistente = ctx.Autores.FirstOrDefault(a => a.Nome == livroAutor.Autor.Nome);
            if (autorExistente == null)
            {
                ctx.Autores.Add(livroAutor.Autor);
            }
        }
    }
    ctx.Livros.Add(livro);
    ctx.SaveChanges();
    return Results.Created("", livro);
});

app.MapDelete("/biblioteca/livro/deletar/{id}", ([FromRoute] string id, [FromServices] AppDataContext ctx) =>{
    Livro? livro = ctx.Livros.Find(id);
    if(livro == null){
        return Results.NotFound();
    }
    ctx.Livros.Remove(livro);
    ctx.SaveChanges();
    return Results.Ok(livro);
});

app.MapPut("/biblioteca/livro/alterar/{id}", ([FromRoute] string id, [FromBody] Livro livroAlterado, [FromServices] AppDataContext ctx) => {
    Livro? livro = ctx.Livros.Find(id);
    if(livro == null){
        return Results.NotFound();
    }
    livro.Titulo = livroAlterado.Titulo;
    livro.QtdExemplares = livroAlterado.QtdExemplares;
    livro.LivrosAutores = livroAlterado.LivrosAutores;
    //Tive que tirar pois estava causando erros no server
    //ctx.Livros.Update(livroAlterado);
    ctx.SaveChanges();
    return Results.Ok(livro);
});

app.MapPost("/biblioteca/autor/cadastrar", ([FromBody] Autor autor, [FromServices] AppDataContext ctx) =>
{
    ctx.Autores.Add(autor);
    ctx.SaveChanges();
    return Results.Created("", autor);
});

app.MapGet("/biblioteca/autor/listar", ([FromServices] AppDataContext ctx) =>
{
    return Results.Ok(ctx.Autores.ToList());
});

app.MapDelete("/biblioteca/autor/deletar/{id}", ([FromRoute] Guid id, [FromServices] AppDataContext ctx) =>
{
    var autor = ctx.Autores.Find(id);
    if (autor == null) return Results.NotFound();

    ctx.Autores.Remove(autor);
    ctx.SaveChanges();
    return Results.Ok(autor);
});


app.UseAuthorization();
app.MapControllers();
app.Run();
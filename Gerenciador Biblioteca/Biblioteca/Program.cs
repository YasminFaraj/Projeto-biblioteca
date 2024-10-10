using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

List<Livro> livros = [
    new Livro() {Titulo = "Os Sete Maridos de Evelyn Hugo", Autor = "Taylor J. R.", qtdExemplares = 3},
    new Livro() {Titulo = "O Morro dos Ventos Uivantes", Autor = "Emily Bronte", qtdExemplares = 2},
];

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

app.MapPost("/biblioteca/livro/cadastrar", ([FromBody] Livro livro,
    [FromServices] AppDataContext ctx) =>
{
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
    livro.qtdExemplares = livroAlterado.qtdExemplares;
    livro.Autor = livroAlterado.Autor;
    //Tive que tirar pois estava causando erros no server
    //ctx.Livros.Update(livroAlterado);
    ctx.SaveChanges();
    return Results.Ok(livro);
});

app.Run();

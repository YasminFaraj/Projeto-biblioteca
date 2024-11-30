using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();

builder.Services.AddCors(options =>
    options.AddPolicy("Acesso Total",
        configs => configs
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod())
);

var app = builder.Build();

app.MapGet("/", () => "API de Livros");

// Listar livro sem autor
app.MapGet("/biblioteca/livro/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Livros.Any())
    {
        return Results.Ok(ctx.Livros.Include(x => x.Autor).ToList());
    }
    return Results.NotFound();
});

// Buscar livro pelo id
app.MapGet("/biblioteca/livro/buscar/{id}", ([FromRoute] string id, [FromServices] AppDataContext ctx) =>{
    Livro? livro = ctx.Livros.Find(id);
    if (livro == null){
        return Results.NotFound();
    } 
    return Results.Ok(livro);
});

// Cadastrar livro
app.MapPost("/biblioteca/livro/cadastrar", ([FromBody] Livro livro, [FromServices] AppDataContext ctx) =>
{
    Autor? autor = ctx.Autores.Find(livro.AutorId);
    if (autor is null)
    {
        return Results.NotFound();
    }
    livro.Autor = autor;
    ctx.Livros.Add(livro);
    ctx.SaveChanges();
    return Results.Created("", livro);
});

// Deletar livro pelo id
app.MapDelete("/biblioteca/livro/deletar/{id}", ([FromRoute] string id, [FromServices] AppDataContext ctx) =>{
    Livro? livro = ctx.Livros.Find(id);
    if(livro == null){
        return Results.NotFound();
    }
    ctx.Livros.Remove(livro);
    ctx.SaveChanges();
    return Results.Ok(livro);
});

// Alterar livro pelo id
app.MapPut("/biblioteca/livro/alterar/{id}", ([FromRoute] string id, [FromBody] Livro livroAlterado, [FromServices] AppDataContext ctx) =>
{
    Livro? livro = ctx.Livros.Find(id);
    if (livro == null)
    {
        return Results.NotFound();
    }
    Autor? autor = ctx.Autores.Find(livroAlterado.AutorId);
    if (autor is null)
    {
        return Results.NotFound();
    }
    livro.Autor = autor;
    livro.Titulo = livroAlterado.Titulo;
    livro.QtdExemplares = livroAlterado.QtdExemplares;
    livro.Genero = livroAlterado.Genero;
    livro.AnoLancamento = livroAlterado.AnoLancamento;
    livro.Editora = livroAlterado.Editora;
    ctx.Livros.Update(livro);
    ctx.SaveChanges();
    return Results.Ok(livro);
});

// Cadastrar autor
app.MapPost("/biblioteca/autor/cadastrar", ([FromBody] Autor autor, [FromServices] AppDataContext ctx) =>
{
    ctx.Autores.Add(autor);
    ctx.SaveChanges();
    return Results.Created("", autor);
});

// Listar autor
app.MapGet("/biblioteca/autor/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Autores.Any()){
        return Results.Ok(ctx.Autores.ToList());
    }    
    return Results.NotFound();
});

// Buscar autor pelo id
app.MapGet("/biblioteca/autor/buscar/{id}", ([FromRoute] string id, [FromServices] AppDataContext ctx) =>{
    Autor? autor = ctx.Autores.Find(id);
    if (autor == null){
        return Results.NotFound();
    } 
    return Results.Ok(autor);
});

// Deletar autor pelo id
app.MapDelete("/biblioteca/autor/deletar/{id}", ([FromRoute] string id, [FromServices] AppDataContext ctx) =>{
    Autor? autor = ctx.Autores.Find(id);
    if(autor == null){
        return Results.NotFound();
    }
    ctx.Autores.Remove(autor);
    ctx.SaveChanges();
    return Results.Ok(autor);
});

// Alterar autor pelo id
app.MapPut("/biblioteca/autor/alterar/{id}", ([FromRoute] Guid id, [FromBody] Autor autorAlterado, [FromServices] AppDataContext ctx) => {
    Autor? autor = ctx.Autores.Find(id);
    if(autor == null){
        return Results.NotFound();
    }
    autor.Nome = autorAlterado.Nome;
    autor.Sobrenome = autorAlterado.Sobrenome;
    autor.Pais = autorAlterado.Pais;
    ctx.SaveChanges();
    return Results.Ok(autor);
});

app.UseCors("Acesso Total");
app.Run();
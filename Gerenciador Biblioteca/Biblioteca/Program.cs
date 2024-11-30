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

// ------ LIVROS -------

// Listar livro com autor
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

// ------ AUTORES -------

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
app.MapPut("/biblioteca/autor/alterar/{id}", ([FromRoute] string id, [FromBody] Autor autorAlterado, [FromServices] AppDataContext ctx) => {
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

// ------ LEITORES -------

// Cadastrar leitor/cliente
app.MapPost("/biblioteca/leitor/cadastrar", ([FromBody] Leitor leitor, [FromServices] AppDataContext ctx) =>
{
    ctx.Leitores.Add(leitor);
    ctx.SaveChanges();
    return Results.Created("", leitor);
});

// Listar leitor/cliente
app.MapGet("/biblioteca/leitor/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Leitores.Any()){
        return Results.Ok(ctx.Leitores.ToList());
    }    
    return Results.NotFound();
});

// Buscar leitor/cliente pelo id
app.MapGet("/biblioteca/leitor/buscar/{id}", ([FromRoute] string id, [FromServices] AppDataContext ctx) =>{
    Leitor? leitor = ctx.Leitores.Find(id);
    if (leitor == null){
        return Results.NotFound();
    } 
    return Results.Ok(leitor);
});

// Deletar leitor/cliente pelo id
app.MapDelete("/biblioteca/leitor/deletar/{id}", ([FromRoute] string id, [FromServices] AppDataContext ctx) =>{
    Leitor? leitor = ctx.Leitores.Find(id);
    if(leitor == null){
        return Results.NotFound();
    }
    ctx.Leitores.Remove(leitor);
    ctx.SaveChanges();
    return Results.Ok(leitor);
});

// Alterar autor pelo id
app.MapPut("/biblioteca/leitor/alterar/{id}", ([FromRoute] string id, [FromBody] Leitor leitorAlterado, [FromServices] AppDataContext ctx) => {
    Leitor? leitor = ctx.Leitores.Find(id);
    if(leitor == null){
        return Results.NotFound();
    }
    leitor.Nome = leitorAlterado.Nome;
    leitor.Sobrenome = leitorAlterado.Sobrenome;
    leitor.Email = leitorAlterado.Email;
    leitor.Telefone = leitorAlterado.Telefone;
    leitor.CPF = leitorAlterado.CPF;
    ctx.SaveChanges();
    return Results.Ok(leitor);
});

app.UseCors("Acesso Total");
app.Run();
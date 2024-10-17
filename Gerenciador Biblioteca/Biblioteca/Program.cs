using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // Manter apenas esta linha
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true; 
        // removi a linha abaixo para evitar conflitos
        // options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

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

// Listar livro sem autor
app.MapGet("/biblioteca/livro/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.Livros.Any()){
        var livrosSemAutores = ctx.Livros
            .Select(l => new{
                l.LivroId,
                l.Titulo,
                l.Genero,
                l.QtdExemplares,
                l.CriadoEm,
                l.AnoLancamento,
                l.Editora
            })
            .ToList();

        return Results.Ok(livrosSemAutores);
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
app.MapPost("/biblioteca/livro/cadastrar", ([FromBody] LivroDTO livroDto, [FromServices] AppDataContext ctx) =>
{
    if (livroDto == null)
    {
        return Results.BadRequest("Dados do livro não podem ser nulos.");
    }

    if (string.IsNullOrEmpty(livroDto.Titulo))
    {
        return Results.BadRequest("O título do livro é obrigatório.");
    }

    var livro = new Livro
    {
        Titulo = livroDto.Titulo,
        Genero = livroDto.Genero,
        QtdExemplares = livroDto.QtdExemplares,
        AnoLancamento = livroDto.AnoLancamento,
        Editora = livroDto.Editora,
        LivrosAutores = new List<LivroAutor>()
    };

    foreach (var livroAutorDto in livroDto.LivrosAutores)
    {
        var autorExistente = ctx.Autores.FirstOrDefault(a => a.AutorId == livroAutorDto.AutorId);
        if (autorExistente != null)
        {
            livro.LivrosAutores.Add(new LivroAutor { Autor = autorExistente, Livro = livro });
        }
        else
        {
            var novoAutor = new Autor
            {
                AutorId = Guid.NewGuid(),
                Nome = livroAutorDto.Nome,
                Sobrenome = livroAutorDto.Sobrenome,
                Pais = livroAutorDto.Pais
            };

            ctx.Autores.Add(novoAutor);

            livro.LivrosAutores.Add(new LivroAutor { Autor = novoAutor, Livro = livro });
        }
    }

    ctx.Livros.Add(livro);

    try
    {
        ctx.SaveChanges();
    }
    catch (Exception ex)
    {
        return Results.Problem("Ocorreu um erro ao cadastrar o livro. Detalhes: " + ex.Message, statusCode: 500);
    }

    return Results.Created($"/biblioteca/livro/{livro.LivroId}", livro);
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
app.MapPut("/biblioteca/livro/alterar/{id}", ([FromRoute] string id, [FromBody] Livro livroAlterado, [FromServices] AppDataContext ctx) => {
    Livro? livro = ctx.Livros.Find(id);
    if(livro == null){
        return Results.NotFound();
    }
    livro.Titulo = livroAlterado.Titulo;
    livro.QtdExemplares = livroAlterado.QtdExemplares;
    livro.Genero = livroAlterado.Genero;
    livro.AnoLancamento = livroAlterado.AnoLancamento;
    livro.Editora = livroAlterado.Editora;
    
    // Remove todos os autores anteriores
    livro.LivrosAutores.Clear();

    // Adiciona os novos autores passados na requisição
    foreach (var livroAutorAlterado in livroAlterado.LivrosAutores)
    {
        // Verifica se o autor já existe no banco
        var autorExistente = ctx.Autores.FirstOrDefault(a => a.AutorId == livroAutorAlterado.AutorId);
        if (autorExistente != null)
        {
            // Cria a relação entre o livro e o autor existente
            livro.LivrosAutores.Add(new LivroAutor { LivroId = livro.LivroId, AutorId = autorExistente.AutorId });
        }
    }

    //Tive que tirar pois estava causando erros no server
    //ctx.Livros.Update(livroAlterado);
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
app.MapGet("/biblioteca/autor/buscar/{id}", ([FromRoute] Guid id, [FromServices] AppDataContext ctx) =>{
    Autor? autor = ctx.Autores.Find(id);
    if (autor == null){
        return Results.NotFound();
    } 
    return Results.Ok(autor);
});

// Deletar autor pelo id
app.MapDelete("/biblioteca/autor/deletar/{id}", ([FromRoute] Guid id, [FromServices] AppDataContext ctx) =>{
    Autor? autor = ctx.Autores.Find(id);
    if(autor == null){
        return Results.NotFound();
    }
    ctx.Autores.Remove(autor);
    ctx.SaveChanges();
    return Results.Ok(autor);
});

// Listar livros com autores
app.MapGet("/biblioteca/livro/listar_com_autores", ([FromServices] AppDataContext ctx) =>
{
    var livrosComAutores = ctx.Livros
        .Include(l => l.LivrosAutores) 
            .ThenInclude(la => la.Autor) 
        .Select(l => new
        {
            l.LivroId,
            l.Titulo,
            l.Genero,
            l.QtdExemplares,
            l.CriadoEm,
            l.AnoLancamento,
            l.Editora,
            Autores = l.LivrosAutores.Select(la => new {
                NomeCompleto = la.Autor!.Nome + " " + la.Autor.Sobrenome
            }).ToList()
        })
        .ToList();

    return Results.Ok(livrosComAutores);
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
    autor.LivrosAutores = autorAlterado.LivrosAutores;
    ctx.SaveChanges();
    return Results.Ok(autor);
});

app.UseAuthorization();
app.MapControllers();
app.Run();
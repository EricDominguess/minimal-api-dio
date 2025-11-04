using MinimalApi.Infraestrutura.Db;
using MinimalApi.DTOs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Dbcontexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();

// GET SIMPLES PARA TESTE
app.MapGet("/", () => "Hello World!");

// POST DE LOGIN PARA VALIDAÇÃO DE USUÁRIO
app.MapPost("/login", (LoginDTO loginDTO) =>
{
  if (loginDTO.Email == "adm@teste.com" && loginDTO.Senha == "123456")
  {
    return Results.Ok("Login realizado com sucesso!");
  }
  else
  {
    return Results.Unauthorized();
  }
});

app.Run();


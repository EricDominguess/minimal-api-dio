using MinimalApi.Infraestrutura.Db;
using MinimalApi.Dominio.DTOs;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Dominio.Servicos;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Entidades;

#region builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Dbcontexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
    );
});

var app = builder.Build();
#endregion

#region Home 
// GET SIMPLES PARA TESTE
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Administradores
// POST DE LOGIN PARA VALIDAÇÃO DE USUÁRIO
app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
{
  if (administradorServico.Login(loginDTO) != null)
  {
    return Results.Ok("Login realizado com sucesso!");
  }
  else
  {
    return Results.Unauthorized();
  }
}).WithTags("Administradores");
#endregion

#region Veiculos

ErrosDeValidacao validaDTO(VeiculoDTO veiculoDTO)
{
  var validacao = new ErrosDeValidacao{
    Mensagens = new List<string>()
  };

  if (string.IsNullOrEmpty(veiculoDTO.Nome))
  {
    validacao.Mensagens.Add("O nome do veiculo é obrigatório.");
  }
  if (string.IsNullOrEmpty(veiculoDTO.Marca))
  {
    validacao.Mensagens.Add("A marca do veiculo é obrigatória.");
  }
  if (veiculoDTO.Ano < 1885)
  {
    validacao.Mensagens.Add("Não existia carros antes de 1885.");
  }

  return validacao;
}

app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
{
  var validacao = validaDTO(veiculoDTO);
  
  if(validacao.Mensagens.Count > 0)
  {
    return Results.BadRequest(validacao);
  }

  var veiculo = new Veiculo
  {
    Nome = veiculoDTO.Nome,
    Marca = veiculoDTO.Marca,
    Ano = veiculoDTO.Ano
  };
  veiculoServico.Incluir(veiculo);

  return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
}).WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculoServico veiculoServico) =>
{
  var veiculos = veiculoServico.Todos(pagina);

  return Results.Ok(veiculos);
}).WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculoServico veiculoServico) =>
{
  var veiculo = veiculoServico.BuscaPorId(id);

  if (veiculo == null)
  {
    return Results.NotFound();
  }
  
  return Results.Ok(veiculo);
  
}).WithTags("Veiculos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, [FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
{
  var veiculo = veiculoServico.BuscaPorId(id);

  if (veiculo == null)
  {
    return Results.NotFound();
  }

  var validacao = validaDTO(veiculoDTO);
  
  if(validacao.Mensagens.Count > 0)
  {
    return Results.BadRequest(validacao);
  }  
  
  veiculo.Nome = veiculoDTO.Nome;
  veiculo.Marca = veiculoDTO.Marca;
  veiculo.Ano = veiculoDTO.Ano;

  veiculoServico.Atualizar(veiculo);

  return Results.Ok(veiculo);

}).WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, [FromBody] VeiculoDTO veiculoDTO, IVeiculoServico veiculoServico) =>
{
  var veiculo = veiculoServico.BuscaPorId(id);

  if (veiculo == null)
  {
    return Results.NotFound();
  }

  veiculoServico.Apagar(veiculo);

  return Results.NoContent();

}).WithTags("Veiculos");

#endregion

#region app
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion

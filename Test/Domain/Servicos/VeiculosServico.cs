using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;
using System.Linq;

namespace Test.Domain.Entidades;

[TestClass]
[DoNotParallelize]
public class VeiculosServicoTest
{
  private Dbcontexto CriarContextoDeTeste()
  {
    var Assenblypath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    var path = Path.GetFullPath(Path.Combine(Assenblypath ?? "", @"..\..\..\..\Test"));

    // Configurar o ConfigurationBuilder
    var builder = new ConfigurationBuilder()
      .SetBasePath(path ?? Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .AddEnvironmentVariables();

    var configuration = builder.Build();

    return new Dbcontexto(configuration);
  }

  [TestMethod]
  public void TestandoSalvarVeiculo()
  {
    // Arrange (todas as variaveis que a gente for usar)
    var context = CriarContextoDeTeste();
    context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos;");

    var veiculo = new Veiculo();
    veiculo.Nome = "Gallardo";
    veiculo.Marca = "Lamborghini";
    veiculo.Ano = 2010;

    var veiculosServico = new VeiculoServico(context);

    // act (a ação que a gente vai executar)
    veiculosServico.Incluir(veiculo);

    // Assert (Fazer a validação da estrutura)
    Assert.AreEqual(1, veiculosServico.Todos(1).Count);

  }
  
  [TestMethod]
  public void TestandoBuscaPorId()
  {
    // Arrange (todas as variaveis que a gente for usar)
    var context = CriarContextoDeTeste();
    context.Database.ExecuteSqlRaw("TRUNCATE TABLE Veiculos;");

    var veiculo = new Veiculo();
    veiculo.Nome = "Gallardo";
    veiculo.Marca = "Lamborghini";
    veiculo.Ano = 2010;

    var veiculosServico = new VeiculoServico(context);

    // act (a ação que a gente vai executar)
    veiculosServico.Incluir(veiculo);
    var veiculoBanco = veiculosServico.BuscaPorId(veiculo.Id);

    // Assert (Fazer a validação da estrutura)
    Assert.IsNotNull(veiculoBanco);
    Assert.AreEqual(1, veiculoBanco.Id);
   
  }
}

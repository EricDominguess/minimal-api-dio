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
public class AdministradorServicoTest
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
  public void TestandoSalvarAdministrador()
  {
    // Arrange (todas as variaveis que a gente for usar)
    var context = CriarContextoDeTeste();
    context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores;");

    var adm = new Administrador();
    adm.Email = "teste@teste.com";
    adm.Senha = "senha123";
    adm.Perfil = "Adm";

    var administradorServico = new AdministradorServico(context);

    // act (a ação que a gente vai executar)
    administradorServico.Incluir(adm);

    // Assert (Fazer a validação da estrutura)
    Assert.AreEqual(1, administradorServico.Todos(1).Count);

  }
  
  [TestMethod]
  public void TestandoBuscaPorId()
  {
    // Arrange (todas as variaveis que a gente for usar)
    var context = CriarContextoDeTeste();
    context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores;");

    var adm = new Administrador();
    adm.Email = "teste@teste.com";
    adm.Senha = "senha123";
    adm.Perfil = "Adm";

    var administradorServico = new AdministradorServico(context);

    // act (a ação que a gente vai executar)
    administradorServico.Incluir(adm);
    var admBanco = administradorServico.BuscarPorId(adm.Id);

    // Assert (Fazer a validação da estrutura)
    Assert.IsNotNull(admBanco);
    Assert.AreEqual(1, admBanco.Id);
   
  }
}

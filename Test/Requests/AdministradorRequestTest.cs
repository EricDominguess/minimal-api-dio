using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Update;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.ModelViews;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class AdministradorRequestTest
{
  [ClassInitialize]
  public static void ClassInit(TestContext testContext)
  {
    Setup.ClassInit(testContext);
  }

  [ClassCleanup]
  public static void ClassCleanup()
  {
    Setup.ClassCleanup();
  }

  [TestMethod]
  public async Task TestarGetSetPropriedades()
  {
    // Arrange (todas as variaveis que a gente for usar)
    var loginDTO = new LoginDTO()
    {
      Email = "adm@teste.com",
      Senha = "123456"
    };

    var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");

    // act (a ação que a gente vai executar)
    var response = await Setup.client.PostAsync("administradores/login", content);


    // Assert (Fazer a validação da estrutura)
    Assert.AreEqual(200, (int)response.StatusCode);

    var result = await response.Content.ReadAsStringAsync();
    var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    });

    Assert.IsNotNull(admLogado?.Email ?? "");
    Assert.IsNotNull(admLogado?.Perfil ?? "");
    Assert.IsNotNull(admLogado?.Token ?? "");

    Console.WriteLine(admLogado?.Token);
  }
}

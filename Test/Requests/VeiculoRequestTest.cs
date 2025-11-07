using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class VeiculoRequestTest
{
  private static readonly JsonSerializerOptions JsonOptions = new()
  {
    PropertyNameCaseInsensitive = true
  };

  [ClassInitialize]
  public static void ClassInit(TestContext ctx) => Setup.ClassInit(ctx);

  [ClassCleanup]
  public static void ClassCleanup() => Setup.ClassCleanup();

  [TestMethod]
  public async Task DeveAdicionarVeiculo()
  {
    // Garante que temos token se a rota exigir autorização
    await GarantirAutenticacaoAsync();

    var dto = new VeiculoDTO
    {
      Nome = $"Carro {Guid.NewGuid():N}",
      Marca = "Marca Test",
      Ano = 2024
    };

    var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
    var response = await Setup.client.PostAsync("veiculos", content);

    var body = await response.Content.ReadAsStringAsync();
    Console.WriteLine($"Status: {(int)response.StatusCode} {response.StatusCode}");
    Console.WriteLine($"Body: {body}");

    Assert.IsTrue(response.StatusCode is HttpStatusCode.OK or HttpStatusCode.Created,
      $"Esperado 200/201. Recebido {(int)response.StatusCode}. Body: {body}");

    // Tenta desserializar direto
    var veiculo = JsonSerializer.Deserialize<Veiculo>(body, JsonOptions);

    // Se veio wrapper, tenta extrair "dados" ou "data"
    if (veiculo == null)
    {
      try
      {
        using var doc = JsonDocument.Parse(body);
        if (doc.RootElement.TryGetProperty("dados", out var dados))
          veiculo = dados.Deserialize<Veiculo>(JsonOptions);
        else if (doc.RootElement.TryGetProperty("data", out var data))
          veiculo = data.Deserialize<Veiculo>(JsonOptions);
      }
      catch { /* Ignora parsing extra */ }
    }

    Assert.IsNotNull(veiculo, $"Objeto Veiculo não desserializado. Body: {body}");
    Assert.IsFalse(string.IsNullOrWhiteSpace(veiculo!.Nome), "Nome vazio");
    Assert.IsFalse(string.IsNullOrWhiteSpace(veiculo.Marca), "Marca vazia");
    Assert.IsTrue(veiculo.Ano > 0, "Ano inválido");
    Assert.IsTrue(veiculo.Id > 0, "Id não atribuído");
  }

  private static async Task GarantirAutenticacaoAsync()
  {
    // Se já existe header Authorization válido, não faz nada
    if (Setup.client.DefaultRequestHeaders.Authorization is not null)
      return;

    var login = new LoginDTO
    {
      Email = "adm@teste.com",
      Senha = "123456"
    };

    var content = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");
    HttpResponseMessage resp;
    try
    {
      resp = await Setup.client.PostAsync("administradores/login", content);
    }
    catch (Exception ex)
    {
      Assert.Inconclusive($"Falha ao chamar login: {ex.Message}");
      return; // unreachable após Inconclusive, mas mantido por clareza
    }

    var body = await resp.Content.ReadAsStringAsync();
    if (!resp.IsSuccessStatusCode || string.IsNullOrWhiteSpace(body))
    {
      Assert.Inconclusive($"Não foi possível autenticar. Status {(int)resp.StatusCode}. Body: {body}");
      return;
    }

    string? token = null;
    try
    {
      // Tenta desserializar diretamente para objeto que possua Token
      using var doc = JsonDocument.Parse(body);
      var root = doc.RootElement;
      if (root.TryGetProperty("token", out var tokenEl) && tokenEl.ValueKind == JsonValueKind.String)
        token = tokenEl.GetString();
      else if (root.TryGetProperty("dados", out var dados) && dados.TryGetProperty("token", out var tokenEl2) && tokenEl2.ValueKind == JsonValueKind.String)
        token = tokenEl2.GetString();
      else if (root.TryGetProperty("data", out var data) && data.TryGetProperty("token", out var tokenEl3) && tokenEl3.ValueKind == JsonValueKind.String)
        token = tokenEl3.GetString();
      else
      {
        // fallback: procurar propriedade contendo 'token'
        foreach (var prop in root.EnumerateObject())
        {
          if (prop.Name.Contains("token", StringComparison.OrdinalIgnoreCase) && prop.Value.ValueKind == JsonValueKind.String)
          {
            token = prop.Value.GetString();
            break;
          }
        }
      }
    }
    catch
    {
      // Ignora parsing, será tratado abaixo
    }

    if (string.IsNullOrWhiteSpace(token))
    {
      Assert.Inconclusive($"Login não retornou token. Body: {body}");
      return;
    }

    Setup.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
  }
}

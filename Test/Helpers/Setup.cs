using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;
using Test.Mocks;
using Microsoft.Extensions.DependencyInjection;
using MinimalApi;

namespace Test.Helpers;

public class Setup
{
  public const string PORT = "5001";
  public static TestContext testContext = default!;
  public static WebApplicationFactory<Startup> http = default!;
  public static HttpClient client = default!;

  public static void ClassInit(TestContext testContext)
  {
    Setup.testContext = testContext;
    Setup.http = new WebApplicationFactory<Startup>();

    Setup.http = Setup.http.WithWebHostBuilder(builder =>
    {
      builder.UseSetting("https_port", Setup.PORT).UseEnvironment("Testing");

      builder.ConfigureServices(services =>
      {
        // Fake auth for tests: autentica sempre como perfil Adm
        services.AddAuthentication(options =>
        {
          options.DefaultAuthenticateScheme = "TestAuth";
          options.DefaultChallengeScheme = "TestAuth";
        })
        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestAuth", options => { });

        services.AddAuthorization(options =>
        {
          // Política fallback exige usuário autenticado pelo esquema de teste
          options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes("TestAuth")
            .RequireAuthenticatedUser()
            .Build();
        });

        services.AddScoped<IAdministradorServico, AdministradorServicoMock>();
        services.AddScoped<IVeiculoServico, VeiculoServicoMock>();
      });
    });

    Setup.client = Setup.http.CreateClient();
  }
  
  public static void ClassCleanup()
  {
    Setup.http.Dispose();
  }
}

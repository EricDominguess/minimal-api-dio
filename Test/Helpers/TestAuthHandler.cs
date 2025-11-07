using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Test.Helpers;

// Autenticador de teste que autentica sempre como usuário com perfil Adm
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
  public TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISystemClock clock) : base(options, logger, encoder, clock)
  { }

  protected override Task<AuthenticateResult> HandleAuthenticateAsync()
  {
    var claims = new[]
    {
      new Claim(ClaimTypes.Name, "test-user"),
      new Claim(ClaimTypes.Role, "Adm"),
      new Claim("Perfil", "Adm"),
      new Claim(ClaimTypes.Email, "adm@teste.com")
    };

    var identity = new ClaimsIdentity(claims, Scheme.Name);
    var principal = new ClaimsPrincipal(identity);
    var ticket = new AuthenticationTicket(principal, Scheme.Name);
    return Task.FromResult(AuthenticateResult.Success(ticket));
  }
}

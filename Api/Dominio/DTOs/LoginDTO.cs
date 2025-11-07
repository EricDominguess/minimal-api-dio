
namespace MinimalApi.Dominio.DTOs;
// CLASS PARA RECEBER DADOS DO LOGIN
public class LoginDTO
{
  public string Email { get; set; } = default!;
  public string Senha { get; set; } = default!;
}

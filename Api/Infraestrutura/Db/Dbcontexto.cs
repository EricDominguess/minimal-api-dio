using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;

namespace MinimalApi.Infraestrutura.Db;

// CONTEXTO DO BANCO DE DADOS
public class Dbcontexto : DbContext
{
  private readonly IConfiguration _configurationAppSettings;
  public Dbcontexto(IConfiguration configurationAppSettings)
  {
    _configurationAppSettings = configurationAppSettings;
  }

  public DbSet<Administrador> Administradores { get; set; } = default!;
  public DbSet<Veiculo> Veiculos { get; set; } = default!;
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Administrador>().HasData(
      new Administrador
      {
        Id = 1,
        Email = "administrador@teste.com",
        Senha = "123456",
        Perfil = "Adm"
      }
    );
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      var stringConexao = _configurationAppSettings.GetConnectionString("MySql")?.ToString();

      if (!string.IsNullOrEmpty(stringConexao))
      {
        optionsBuilder.UseMySql(stringConexao,
          ServerVersion.AutoDetect(stringConexao));
      }
    }
  }
}

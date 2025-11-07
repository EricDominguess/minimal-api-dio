using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;

namespace Test.Mocks;

public class VeiculoServicoMock : IVeiculoServico
{
  private static List<Veiculo> veiculos = new List<Veiculo>()
  {
    new Veiculo()
    {
      Id = 1,
      Nome = "Carro A",
      Marca = "Marca A",
      Ano = 2020
    },
    new Veiculo()
    {
      Id = 2,
      Nome = "Carro B",
      Marca = "Marca B",
      Ano = 2021
    }
  };

  public void Apagar(Veiculo veiculo)
  {
    veiculos.Remove(veiculo);
  }

  public void Atualizar(Veiculo veiculo)
  {
    var index = veiculos.FindIndex(v => v.Id == veiculo.Id);
    if (index != -1)
    {
      veiculos[index] = veiculo;
    }
  }

  public Veiculo? BuscaPorId(int id)
  {
    return veiculos.Find(v => v.Id == id);
  }

  public void Incluir(Veiculo veiculo)
  {
    veiculo.Id = veiculos.Count() + 1;
    veiculos.Add(veiculo);
  }

  public List<Veiculo> Todos(int? pagina = 1, string nome = null, string marca = null)
  {
    return veiculos;
  }
}

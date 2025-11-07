using MinimalApi.Dominio.Entidades;

namespace Test.Domain.Entidades;

[TestClass]
public class VeiculoTest
{
  [TestMethod]
  public void TestarGetSetPropriedades()
  {
    // Arrange (todas as variaveis que a gente for usar)
    var veiculo = new Veiculo();

    // act (a ação que a gente vai executar)
    veiculo.Id = 1;
    veiculo.Nome = "Fusca";
    veiculo.Marca = "Volkswagen";
    veiculo.Ano = 1968;


    // Assert (Fazer a validação da estrutura)
    Assert.AreEqual(1, veiculo.Id);
    Assert.AreEqual("Fusca", veiculo.Nome);
    Assert.AreEqual("Volkswagen", veiculo.Marca);
    Assert.AreEqual(1968, veiculo.Ano);
  }
}

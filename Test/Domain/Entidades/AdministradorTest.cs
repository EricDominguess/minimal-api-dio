using MinimalApi.Dominio.Entidades;

namespace Test.Domain.Entidades;

[TestClass]
public class AdministradorTest
{
  [TestMethod]
  public void TestarGetSetPropriedades()
  {
    // Arrange (todas as variaveis que a gente for usar)
    var adm = new Administrador();

    // act (a ação que a gente vai executar)
    adm.Id = 1;
    adm.Email = "teste@teste.com";
    adm.Senha = "senha123";
    adm.Perfil = "Adm";

    // Assert (Fazer a validação da estrutura)
    Assert.AreEqual(1, adm.Id);
    Assert.AreEqual("teste@teste.com", adm.Email);
    Assert.AreEqual("senha123", adm.Senha);
    Assert.AreEqual("Adm", adm.Perfil);
  }
}

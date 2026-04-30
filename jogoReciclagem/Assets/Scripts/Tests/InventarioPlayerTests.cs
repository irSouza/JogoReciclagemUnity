using NUnit.Framework;

namespace Tests
{
    public class InventarioPlayerTests
    {
        [Test]
        public void CalcularDescarte_AcertouLixeiraOrganica_GanhaPontosEGastaLixo()
        {
            // Arrange
            int qtdOrganico = 1;
            int qtdReciclavel = 0;
            int pontos = 0;

            // Act
            bool alterou = InventarioPlayer.CalcularDescarte(
                TipoLixo.TipoDeLixo.Organico,
                TipoLixo.TipoDeLixo.Organico,
                ref qtdOrganico,
                ref qtdReciclavel,
                ref pontos,
                out bool sucesso
            );

            // Assert
            Assert.IsTrue(alterou, "Deveria ter alterado o estado.");
            Assert.IsTrue(sucesso, "Deveria ser um descarte com sucesso.");
            Assert.AreEqual(10, pontos, "Deveria ganhar 10 pontos.");
            Assert.AreEqual(0, qtdOrganico, "A quantidade de lixo orgânico deveria diminuir.");
            Assert.AreEqual(0, qtdReciclavel, "A quantidade de lixo reciclável não deveria mudar.");
        }

        [Test]
        public void CalcularDescarte_AcertouLixeiraReciclavel_GanhaPontosEGastaLixo()
        {
            // Arrange
            int qtdOrganico = 0;
            int qtdReciclavel = 1;
            int pontos = 0;

            // Act
            bool alterou = InventarioPlayer.CalcularDescarte(
                TipoLixo.TipoDeLixo.Reciclavel,
                TipoLixo.TipoDeLixo.Reciclavel,
                ref qtdOrganico,
                ref qtdReciclavel,
                ref pontos,
                out bool sucesso
            );

            // Assert
            Assert.IsTrue(alterou);
            Assert.IsTrue(sucesso);
            Assert.AreEqual(10, pontos);
            Assert.AreEqual(0, qtdOrganico);
            Assert.AreEqual(0, qtdReciclavel);
        }

        [Test]
        public void CalcularDescarte_ErrouLixeira_PerdePontosEGastaLixo()
        {
            // Arrange
            int qtdOrganico = 1;
            int qtdReciclavel = 0;
            int pontos = 10;

            // Act
            // Tentou jogar Orgânico na lixeira de Reciclável
            bool alterou = InventarioPlayer.CalcularDescarte(
                TipoLixo.TipoDeLixo.Organico,
                TipoLixo.TipoDeLixo.Reciclavel,
                ref qtdOrganico,
                ref qtdReciclavel,
                ref pontos,
                out bool sucesso
            );

            // Assert
            Assert.IsTrue(alterou);
            Assert.IsFalse(sucesso, "Descarte incorreto não deveria retornar sucesso.");
            Assert.AreEqual(5, pontos, "Deveria perder 5 pontos.");
            Assert.AreEqual(0, qtdOrganico, "O lixo orgânico deveria ser gasto de qualquer forma.");
            Assert.AreEqual(0, qtdReciclavel);
        }

        [Test]
        public void CalcularDescarte_ErrouLixeira_PontosNaoFicamNegativos()
        {
            // Arrange
            int qtdOrganico = 0;
            int qtdReciclavel = 1;
            int pontos = 3;

            // Act
            // Tentou jogar Reciclável na lixeira de Orgânico
            bool alterou = InventarioPlayer.CalcularDescarte(
                TipoLixo.TipoDeLixo.Reciclavel,
                TipoLixo.TipoDeLixo.Organico,
                ref qtdOrganico,
                ref qtdReciclavel,
                ref pontos,
                out bool sucesso
            );

            // Assert
            Assert.IsTrue(alterou);
            Assert.IsFalse(sucesso);
            Assert.AreEqual(0, pontos, "Os pontos não deveriam ficar menores que 0.");
            Assert.AreEqual(0, qtdOrganico);
            Assert.AreEqual(0, qtdReciclavel, "O lixo reciclável deveria ser gasto.");
        }

        [Test]
        public void CalcularDescarte_SemLixoNaMochila_NaoAlteraEstado()
        {
            // Arrange
            int qtdOrganico = 0;
            int qtdReciclavel = 0;
            int pontos = 10;

            // Act
            bool alterou = InventarioPlayer.CalcularDescarte(
                TipoLixo.TipoDeLixo.Organico,
                TipoLixo.TipoDeLixo.Organico,
                ref qtdOrganico,
                ref qtdReciclavel,
                ref pontos,
                out bool sucesso
            );

            // Assert
            Assert.IsFalse(alterou, "Não deve alterar o estado se não tem o lixo.");
            Assert.IsFalse(sucesso);
            Assert.AreEqual(10, pontos, "Pontos não devem mudar.");
            Assert.AreEqual(0, qtdOrganico);
            Assert.AreEqual(0, qtdReciclavel);
        }
    }
}

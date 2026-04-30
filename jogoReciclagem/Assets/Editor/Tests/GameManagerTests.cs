using NUnit.Framework;

namespace Tests
{
    public class GameManagerTests
    {
        [TestCase(-10, ExpectedResult = 0)]
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(10, ExpectedResult = 1)]
        [TestCase(49, ExpectedResult = 1)]
        [TestCase(50, ExpectedResult = 2)]
        [TestCase(99, ExpectedResult = 2)]
        [TestCase(100, ExpectedResult = 3)]
        [TestCase(150, ExpectedResult = 3)]
        public int CalcularEstrelas_RetornaQuantidadeCorreta(int pontos)
        {
            return GameManager.CalcularEstrelas(pontos);
        }
    }
}

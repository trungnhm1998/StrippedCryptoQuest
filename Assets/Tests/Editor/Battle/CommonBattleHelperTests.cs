using CryptoQuest.Gameplay.Battle.Helper;
using NUnit.Framework;

namespace CryptoQuest.Tests.Editor.Battle
{
    public class CommonBattleHelperTests
    {
        private readonly int[] _weights = new int[4] {10, 8, 6, 5};

        [Test]
        [TestCase(0, 9)]
        [TestCase(1, 17)]
        [TestCase(2, 23)]
        [TestCase(3, 28)]
        public void WeightedRandomIndex_ShouldCorrect(int expectedIndex, int randomSeed)
        {
            var resultIndex = CommonBattleHelper.WeightedRandomIndex(_weights, randomSeed);
            Assert.AreEqual(expectedIndex, resultIndex);
        }
    }
}

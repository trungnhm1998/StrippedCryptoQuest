using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation;
using NUnit.Framework;

namespace CryptoQuest.Tests.Editor.Battle
{
    public class BattlePhysicalDamageCalculatorTests
    {
        [Test]
        [TestCase(10, 10, 10, 5, 5, 25)]
        [TestCase(0, 0, 10, 5, 5, 0)]
        [TestCase(10, 0, 10, 5, 5, 0)]
        [TestCase(10, 10, 10, 5, 0, 125)]
        [TestCase(10, 10, 10, 0, 5, 0)]
        [TestCase(10, 10, 10, 0, 0, 0)]
        [TestCase(10, 10, 0, 5, 5, 50)]
        public void BattleCalculatorPercentPhysical_CalculateValidParams_ReturnCorrectValue(float baseDamage,
            float attack, float defence, float elementAttack, float elementResist, float expectedDamage)

        {
            float calculatedValue =
                BattleCalculator.CalculatePercentPhysicalDamage(baseDamage, attack, defence, elementAttack,
                    elementResist);

            Assert.AreEqual(expectedDamage, calculatedValue);
        }

        [Test]
        [TestCase(10, 10, 10, 10)]
        [TestCase(0, 0, 10, 0)]
        [TestCase(10, 0, 10, 0)]
        [TestCase(10, 10, 0, 100)]
        public void BattleCalculatorFixedPhysical_CalculateValidParams_ReturnCorrectValue(float baseDamage,
            float elementAttack, float elementResist, float expectedDamage)

        {
            float calculatedValue =
                BattleCalculator.CalculateFixedPhysicalDamage(baseDamage, elementAttack, elementResist);

            Assert.AreEqual(expectedDamage, calculatedValue);
        }
    }
}
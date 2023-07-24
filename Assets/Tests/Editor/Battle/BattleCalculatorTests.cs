using CryptoQuest.Gameplay.Battle.Core;
using NUnit.Framework;

namespace CryptoQuest.Tests.Editor.Battle
{
    public class BattleCalculatorTests
    {
        [Test]
        public void BattleCalculator_CalculateValidParams_ReturnCorrectValue()
        {
            BattleCalculator calculator = new();
            SkillParameters skillParameters = new()
            {
                BasePower = 10,
                SkillPowerThreshold = 10,
                PowerValueAdded = 0.5f,
                PowerLowerLimit = 10,
                PowerUpperLimit = 20
            };
            float calculatedValue = calculator.CalculateBaseDamage(skillParameters, 10, 0.01f);
            Assert.AreEqual(10.1f, calculatedValue);
        }

        [Test]
        public void BattleCalculator_CalculateValidParams_ReturnPowerLowerLimit()
        {
            BattleCalculator calculator = new();
            SkillParameters skillParameters = new()
            {
                BasePower = 0,
                SkillPowerThreshold = 10,
                PowerValueAdded = 0.5f,
                PowerLowerLimit = 10,
                PowerUpperLimit = 20
            };
            float randomValue = 0f;
            float calculatedValue = calculator.CalculateBaseDamage(skillParameters, 0, randomValue);
            float expectedValue = skillParameters.PowerLowerLimit + skillParameters.PowerLowerLimit * randomValue;
            Assert.AreEqual(expectedValue, calculatedValue);
        }

        [Test]
        public void BattleCalculator_CalculateValidParams_ReturnPowerUpperLimit()
        {
            BattleCalculator calculator = new();
            SkillParameters skillParameters = new()
            {
                BasePower = 100,
                SkillPowerThreshold = 10,
                PowerValueAdded = 0.5f,
                PowerLowerLimit = 10,
                PowerUpperLimit = 20
            };
            float randomValue = 0.01f;
            float calculatedValue = calculator.CalculateBaseDamage(skillParameters, 100, randomValue);
            float expectedValue = skillParameters.PowerUpperLimit + skillParameters.PowerUpperLimit * randomValue;
            Assert.AreEqual(expectedValue, calculatedValue);
        }

        [Test]
        public void BattleCalculator_CalculateWithNegativeAttackPower_ReturnPowerLowerLimit()
        {
            BattleCalculator calculator = new();
            SkillParameters skillParameters = new()
            {
                BasePower = 0,
                SkillPowerThreshold = 10,
                PowerValueAdded = 0.5f,
                PowerLowerLimit = 10,
                PowerUpperLimit = 20
            };
            float randomValue = 0.01f;
            float calculatedValue = calculator.CalculateBaseDamage(skillParameters, -100, randomValue);
            float expectedValue = skillParameters.PowerLowerLimit + skillParameters.PowerLowerLimit * randomValue;
            Assert.AreEqual(expectedValue, calculatedValue);
        }
    }
}
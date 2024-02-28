using CryptoQuest.AbilitySystem;
using CryptoQuest.AbilitySystem.Executions;
using NUnit.Framework;

namespace CryptoQuest.Tests.Editor.Battle
{
    public class BattleMagicDamageCalculatorTests
    {
        [Test]
        public void BattleCalculator_CalculateValidParams_ReturnCorrectValue()
        {
            SkillParameterBuilder builder = new();

            SkillParameters skillParameters = builder
                .WithBasePower(10)
                .WithPowerUpperLimit(20)
                .WithPowerLowerLimit(10)
                .WithSkillPowerThreshold(10)
                .WithPowerValueAdded(0.5f)
                .Build();

            float calculatedValue = BattleCalculator.CalculateMagicSkillBasePower(skillParameters, 10);
            calculatedValue.Offset(0.01f);
            Assert.AreEqual(10.1f, calculatedValue);
        }

        [Test]
        public void BattleCalculator_CalculateValidParams_ReturnPowerLowerLimit()
        {
            SkillParameterBuilder builder = new();

            SkillParameters skillParameters = builder
                .WithBasePower(0)
                .Build();

            float randomValue = 0f;
            float calculatedValue = BattleCalculator.CalculateMagicSkillBasePower(skillParameters, 0);
            calculatedValue.Offset(randomValue);
            float expectedValue = skillParameters.PowerLowerLimit + skillParameters.PowerLowerLimit * randomValue;
            Assert.AreEqual(expectedValue, calculatedValue);
        }

        [Test]
        public void BattleCalculator_CalculateValidParams_ReturnPowerUpperLimit()
        {
            SkillParameterBuilder builder = new();

            SkillParameters skillParameters = builder
                .WithBasePower(100)
                .Build();

            float randomValue = 0.01f;
            float calculatedValue = BattleCalculator.CalculateMagicSkillBasePower(skillParameters, 100);
            calculatedValue.Offset(randomValue);
            float expectedValue = skillParameters.PowerUpperLimit + skillParameters.PowerUpperLimit * randomValue;
            Assert.AreEqual(expectedValue, calculatedValue);
        }

        [Test]
        public void BattleCalculator_CalculateWithNegativeAttackPower_ReturnPowerLowerLimit()
        {
            SkillParameterBuilder builder = new();

            SkillParameters skillParameters = builder
                .WithBasePower(0)
                .Build();

            float randomValue = 0.01f;
            float calculatedValue = BattleCalculator.CalculateMagicSkillBasePower(skillParameters, -100);
            calculatedValue.Offset(randomValue);
            float expectedValue = skillParameters.PowerLowerLimit + skillParameters.PowerLowerLimit * randomValue;
            Assert.AreEqual(expectedValue, calculatedValue);
        }

        [Test]
        public void BattleCalculator_CalculateWithNegativeBasePower_ReturnPowerLowerLimit()
        {
            SkillParameterBuilder builder = new();

            SkillParameters skillParameters = builder
                .WithBasePower(-100)
                .Build();

            float randomValue = 0.01f;
            float calculatedValue = BattleCalculator.CalculateMagicSkillBasePower(skillParameters, 10);
            calculatedValue.Offset(randomValue);
            float expectedValue = skillParameters.PowerLowerLimit + skillParameters.PowerLowerLimit * randomValue;
            Assert.AreEqual(expectedValue, calculatedValue);
        }

        [Test]
        public void BattleCalculator_CalculateWithNegativeBasePowerAndAttackPower_ReturnPowerLowerLimit()
        {
            SkillParameterBuilder builder = new();

            SkillParameters skillParameters = builder
                .WithBasePower(-100)
                .Build();

            float randomValue = 0.01f;
            float calculatedValue = BattleCalculator.CalculateMagicSkillBasePower(skillParameters, -100);
            calculatedValue.Offset(randomValue);
            float expectedValue = skillParameters.PowerLowerLimit + skillParameters.PowerLowerLimit * randomValue;
            Assert.AreEqual(expectedValue, calculatedValue);
        }
    }
}
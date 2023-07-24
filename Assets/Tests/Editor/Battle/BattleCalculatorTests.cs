using System;
using CryptoQuest.Gameplay.Battle.Core;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation;
using NUnit.Framework;

namespace CryptoQuest.Tests.Editor.Battle
{
    public class BattleCalculatorTests
    {
        [Test]
        public void BattleCalculator_CalculateValidParams_ReturnCorrectValue()
        {
            BattleCalculator calculator = new();
            SkillParameterBuilder builder = new();

            SkillParameters skillParameters = builder
                .WithBasePower(10)
                .WithPowerUpperLimit(20)
                .WithPowerLowerLimit(10)
                .WithSkillPowerThreshold(10)
                .WithPowerValueAdded(0.5f)
                .Build();

            float calculatedValue = BattleCalculator.CalculateBaseDamage(skillParameters, 10, 0.01f);
            Assert.AreEqual(10.1f, calculatedValue);
        }

        [Test]
        public void BattleCalculator_CalculateValidParams_ReturnPowerLowerLimit()
        {
            BattleCalculator calculator = new();
            SkillParameterBuilder builder = new();

            SkillParameters skillParameters = builder
                .WithBasePower(0)
                .WithPowerUpperLimit(20)
                .WithPowerLowerLimit(10)
                .WithSkillPowerThreshold(10)
                .WithPowerValueAdded(0.5f)
                .Build();

            float randomValue = 0f;
            float calculatedValue = BattleCalculator.CalculateBaseDamage(skillParameters, 0, randomValue);
            float expectedValue = skillParameters.PowerLowerLimit + skillParameters.PowerLowerLimit * randomValue;
            Assert.AreEqual(expectedValue, calculatedValue);
        }

        [Test]
        public void BattleCalculator_CalculateValidParams_ReturnPowerUpperLimit()
        {
            BattleCalculator calculator = new();

            SkillParameterBuilder builder = new();

            SkillParameters skillParameters = builder
                .WithBasePower(100)
                .WithPowerUpperLimit(20)
                .WithPowerLowerLimit(10)
                .WithSkillPowerThreshold(10)
                .WithPowerValueAdded(0.5f)
                .Build();

            float randomValue = 0.01f;
            float calculatedValue = BattleCalculator.CalculateBaseDamage(skillParameters, 100, randomValue);
            float expectedValue = skillParameters.PowerUpperLimit + skillParameters.PowerUpperLimit * randomValue;
            Assert.AreEqual(expectedValue, calculatedValue);
        }

        [Test]
        public void BattleCalculator_CalculateWithNegativeAttackPower_ReturnPowerLowerLimit()
        {
            BattleCalculator calculator = new();
            SkillParameterBuilder builder = new();

            SkillParameters skillParameters = builder
                .WithBasePower(0)
                .WithPowerUpperLimit(20)
                .WithPowerLowerLimit(10)
                .WithSkillPowerThreshold(10)
                .WithPowerValueAdded(0.5f)
                .Build();

            float randomValue = 0.01f;
            float calculatedValue = BattleCalculator.CalculateBaseDamage(skillParameters, -100, randomValue);
            float expectedValue = skillParameters.PowerLowerLimit + skillParameters.PowerLowerLimit * randomValue;
            Assert.AreEqual(expectedValue, calculatedValue);
        }

        [Test]
        public void BattleCalculator_CalculateWithNegativeBasePower_ReturnPowerLowerLimit()
        {
            BattleCalculator calculator = new();
            SkillParameterBuilder builder = new();

            SkillParameters skillParameters = builder
                .WithBasePower(-100)
                .WithPowerUpperLimit(20)
                .WithPowerLowerLimit(10)
                .WithSkillPowerThreshold(10)
                .WithPowerValueAdded(0.5f)
                .Build();

            float randomValue = 0.01f;
            float calculatedValue = BattleCalculator.CalculateBaseDamage(skillParameters, 10, randomValue);
            float expectedValue = skillParameters.PowerLowerLimit + skillParameters.PowerLowerLimit * randomValue;
            Assert.AreEqual(expectedValue, calculatedValue);
        }

        [Test]
        public void BattleCalculator_CalculateWithNegativeBasePowerAndAttackPower_ReturnPowerLowerLimit()
        {
            BattleCalculator calculator = new();
            SkillParameterBuilder builder = new();

            SkillParameters skillParameters = builder
                .WithBasePower(-100)
                .WithPowerUpperLimit(20)
                .WithPowerLowerLimit(10)
                .WithSkillPowerThreshold(10)
                .WithPowerValueAdded(0.5f)
                .Build();

            float randomValue = 0.01f;
            float calculatedValue = BattleCalculator.CalculateBaseDamage(skillParameters, -100, randomValue);
            float expectedValue = skillParameters.PowerLowerLimit + skillParameters.PowerLowerLimit * randomValue;
            Assert.AreEqual(expectedValue, calculatedValue);
        }
    }
}
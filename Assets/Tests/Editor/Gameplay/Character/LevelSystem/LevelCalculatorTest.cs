using CryptoQuest.Character.Movement;
using NUnit.Framework;
using UnityEngine;
using System.IO;
using CryptoQuest.Character.LevelSystem;

namespace CryptoQuest.Tests.Editor.Gameplay.Character.LevelSystem
{
    [TestFixture]
    public class LevelCalculatorTest
    {
        private const int ROW_OFFSET = 4;
        private string _calculatedLevelExpPath = "Assets/Tests/Editor/Gameplay/Character/LevelSystem/calculated.tsv"; 
        private int[] _calculatedRequiredExps;
        private int[] _calculatedAccumulateExps;

        private ILevelCalculator _calculator;

        [SetUp]
        public void Setup()
        {
            string[] allLines = File.ReadAllLines(_calculatedLevelExpPath);

            var maxLevel = allLines.Length - ROW_OFFSET;
            Debug.Log($"Max level = {maxLevel}");

            _calculatedRequiredExps = new int[maxLevel];
            _calculatedAccumulateExps = new int[maxLevel];

            for (int index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splitedData = allLines[index].Split('\t');

                if (int.TryParse(splitedData[1], out var parsedRequire))
                {
                    Debug.Log($"Calculated Required EXP Level {index - ROW_OFFSET + 1}: {parsedRequire}");
                    _calculatedRequiredExps[index - ROW_OFFSET] = parsedRequire;
                }

                if (int.TryParse(splitedData[1], out var parsedAccumulate))
                {
                    _calculatedAccumulateExps[index - ROW_OFFSET] = parsedAccumulate;
                }
            }

            _calculator = new LevelCalculator(maxLevel);
        }

        [Test]
        public void LevelCalculator_RequiredExps_ShouldAllEqual()
        {
            var _requiredExps = _calculator.RequiredExps;
            for (int i = 0; i < _calculatedRequiredExps.Length; i++)
            {
                Assert.AreEqual(_calculatedRequiredExps[i], _requiredExps[i]);
            }
        }

        [Test]
        [TestCase(0f, 1)]
        [TestCase(39f, 1)]
        [TestCase(40f, 2)]
        [TestCase(60f, 2)]
        [TestCase(169f, 3)]
        [TestCase(7289943f, 99)]
        [TestCase(7289944f, 99)]
        public void LevelCalculator_CalculateCurrentLevel_ShouldReturnCorrectLevel(float currentExp, int expectedLevel)
        {
            Assert.AreEqual(expectedLevel, _calculator.CalculateCurrentLevel(currentExp));
        }
    }
}
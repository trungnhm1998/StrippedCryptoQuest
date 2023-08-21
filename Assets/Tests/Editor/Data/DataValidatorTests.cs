using System.Collections;
using CryptoQuest.Data;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Editor.Data
{
    public class DataValidatorTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void DataValidator_WithValidParams_ReturnCorrect()
        {
            var a = new MonsterUnitDataModel()
            {
                MonsterId = 1,
                MonsterName = "Test",
                ElementId = 1,
                HP = 100,
                MP = 100,
                Strength = 100,
                Vitality = 100,
                Agility = 100,
                Intelligence = 100,
                Luck = 100,
                Attack = 100,
                SkillPower = 100,
                Defense = 100,
                CriticalRate = 100,
                Exp = 100,
                Gold = 100,
                DropItemID = "1"
            };
            Assert.IsTrue(DataValidator.MonsterDataValidator(a));
        }
    }
}
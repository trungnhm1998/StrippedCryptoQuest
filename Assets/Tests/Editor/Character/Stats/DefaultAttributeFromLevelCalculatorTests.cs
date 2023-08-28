using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Inventory.Items;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Character.Stats
{
    [TestFixture]
    public class DefaultAttributeFromLevelCalculatorTests
    {
        /// <summary>
        /// With the current formula, the value at level 10 should be 186.816 but it floor to 186 instead
        /// This cause the player .816 HP less than expected
        /// </summary>
        [Test]
        public void GetValueAtLevel_WithLevel10MinHP155MaxHP470_ShouldReturn185()
        {
            var defaultLevelCalculator = new CryptoQuest.Gameplay.Inventory.Items.DefaultAttributeFromLevelCalculator();
            var cappedAttributeDef = new CappedAttributeDef()
            {
                Attribute = ScriptableObject.CreateInstance<AttributeScriptableObject>(),
                MinValue = 155,
                MaxValue = 470,
            };
            var attributeDefs = new CappedAttributeDef[]
            {
                cappedAttributeDef
            };
            var stats = new StatsDef()
            {
                Attributes = attributeDefs,
                MaxLevel = 99
            };
            var spec = new CharacterSpec()
            {
                StatsDef = stats,
                Level = 10
            };

            var value = defaultLevelCalculator.GetValueAtLevel(10, cappedAttributeDef, stats);

            Assert.AreEqual(186, value);
        }
    }
}
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Gameplay.Character.LevelSystem
{
    [TestFixture]
    public class LevelSystemTests
    {
        private HeroBehaviour _hero;
        private CryptoQuest.Battle.Components.LevelSystem _lvlSystem;
        private AttributeSystemBehaviour _attributeSystem;

        [SetUp]
        public void Setup()
        {
            var gameObject = new GameObject();
            _attributeSystem = gameObject.AddComponent<AttributeSystemBehaviour>();
            _hero = gameObject.AddComponent<HeroBehaviour>();
            _lvlSystem = gameObject.AddComponent<CryptoQuest.Battle.Components.LevelSystem>();
        }

        [TestCase(0f, 1)]
        [TestCase(39f, 1)]
        [TestCase(40f, 2)]
        [TestCase(60f, 2)]
        [TestCase(169f, 3)]
        [TestCase(7289943f, 99)]
        public void Level_WithMaxLevelOf99_ShouldReturn(float exp, int expectedLevel)
        {
            _hero.Spec = new HeroSpec()
            {
                Experience = exp,
                Stats = new StatsDef
                {
                    MaxLevel = 99
                }
            };

            Assert.AreEqual(expectedLevel, _lvlSystem.Level);
        }
        
        [TestCase(10f, 1)]
        public void AddExp_WithExpToAdd_ShouldAddExp(float expToAdd, int expectedLevel)
        {
            _hero.Spec = new HeroSpec()
            {
                Experience = 0,
                Stats = new StatsDef
                {
                    MaxLevel = 99
                }
            };
            
            _lvlSystem.AddExp(expToAdd);
            
            Assert.AreEqual(expectedLevel, _lvlSystem.Level);
        }
    }
}
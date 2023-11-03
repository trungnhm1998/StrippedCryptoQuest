using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Gameplay.Character.LevelSystem
{
    [TestFixture]
    public class LevelSystemTests
    {
        [TestCase(0f, 1)]
        [TestCase(39f, 1)]
        [TestCase(40f, 2)]
        [TestCase(60f, 2)]
        [TestCase(169f, 3)]
        [TestCase(7289943f, 99)]
        public void Level_WithMaxLevelOf99_ShouldReturn(float exp, int expectedLevel)
        {
            var gameObject = new GameObject();
            var hero = gameObject.AddComponent<HeroBehaviour>();
            var lvlSystem = gameObject.AddComponent<CryptoQuest.Battle.Components.LevelSystem>();

            hero.Spec = new HeroSpec()
            {
                Experience = exp,
                Stats = new StatsDef
                {
                    MaxLevel = 99
                }
            };

            Assert.AreEqual(expectedLevel, lvlSystem.Level);
        }
    }
}
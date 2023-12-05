using System.Collections;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Item.MagicStone;
using CryptoQuest.UI.Utilities;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Editor.Item.MagicStone
{
    [TestFixture]
    public class MagicStonePassiveControllerTests
    {
        private MagicStonePassiveController _controller;
        private HeroBehaviour _hero;

        [UnitySetUp]
        public IEnumerator Setup()
        {
            var heroPrefab = AssetDatabase.LoadAssetAtPath<HeroBehaviour>("Assets/Prefabs/Battle/HeroGAS.prefab");
            _hero = Object.Instantiate(heroPrefab);
            _controller = _hero.GetOrAddComponent<MagicStonePassiveController>();
            yield return null;
            _controller.Init();
        }

        [Test]
        public void ApplyPassives_ApplyCorrectPassives()
        {
            var stone = Substitute.For<IMagicStone>();
            stone.Passives.Returns(new[]
            {
                ScriptableObject.CreateInstance<PassiveAbility>(),
                ScriptableObject.CreateInstance<PassiveAbility>()
            });

            _controller.ApplyPassives(stone);

            Assert.AreEqual(2, _hero.AbilitySystem.GrantedAbilities.Count);
        }

        [Test]
        public void ApplyPassives_WithSameStone_ShouldNotApply()
        {
            var stone = Substitute.For<IMagicStone>();
            stone.Passives.Returns(new[]
            {
                ScriptableObject.CreateInstance<PassiveAbility>(),
                ScriptableObject.CreateInstance<PassiveAbility>()
            });

            _controller.ApplyPassives(stone);
            _controller.ApplyPassives(stone);

            Assert.AreEqual(2, _hero.AbilitySystem.GrantedAbilities.Count);
        }

        [Test]
        public void RemovePassive_RemoveCorrectPassives()
        {
            var stone = Substitute.For<IMagicStone>();
            stone.Passives.Returns(new[]
            {
                ScriptableObject.CreateInstance<PassiveAbility>(),
                ScriptableObject.CreateInstance<PassiveAbility>()
            });

            _controller.ApplyPassives(stone);
            Assert.AreEqual(2, _hero.AbilitySystem.GrantedAbilities.Count);
            _controller.RemovePassives(stone);

            Assert.AreEqual(0, _hero.AbilitySystem.GrantedAbilities.Count);
        }
    }
}
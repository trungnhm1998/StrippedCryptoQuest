using CryptoQuest.Battle;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Runtime.Battle
{
    public class CharacterTests : FixtureBase
    {
        private const string CHARACTER_PREFAB_ASSET = "Assets/Prefabs/Battle/Character.prefab";
        private GameObject _characterGameObject;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var attributeSet = AssetDatabase
                .LoadAllAssetsAtPath("Assets/ScriptableObjects/Character/Attributes/AttributeSets.asset")[0];
        }

        [SetUp]
        public void SetUp()
        {
            _characterGameObject = CreateGameObjectFromPrefab(CHARACTER_PREFAB_ASSET);
        }

        [Test]
        public void CharacterShouldHaveGameplayAbilitySystem()
        {
            var character = _characterGameObject.GetComponent<ICharacter>();
            Assert.NotNull(character);
            character.Init();
            Assert.NotNull(character.Attributes);
        }

        [TestFixture]
        public class SimpleStatsInitializerTests : CharacterTests
        {
            [Test]
            public void Init_ShouldSetStats()
            {
                var character = _characterGameObject.GetComponent<ICharacter>();
                var statsInitializer = _characterGameObject.GetComponent<IStatsInitializer>();
                statsInitializer.Init(character.Attributes);
                character.Attributes.TryGetAttributeValue(AttributeSets.Health, out var health);
                Assert.AreEqual(100f, health.CurrentValue);

                // We set strength to 50 in the prefab, but atk is calculated from strength, so it should be 50 too.
                character.Attributes.TryGetAttributeValue(AttributeSets.Attack, out var atk);
                Assert.AreEqual(50f, atk.CurrentValue);
            }
        }

        [TestFixture]
        public class NormalAttackAbilityTests : CharacterTests
        {
            [Test]
            public void Attack_Hero_HealthShouldBeLessThanWhenInit()
            {
                var hero = _characterGameObject.GetComponent<ICharacter>();
                hero.Element =
                    AssetDatabase.LoadAssetAtPath<Elemental>(
                        "Assets/ScriptableObjects/Character/Attributes/Elemental/Fire/Fire.asset");
                hero.Init();
                hero.Attributes.TryGetAttributeValue(AttributeSets.Health, out var heroHealth);
                Assert.AreEqual(100f, heroHealth.CurrentValue);
                var enemy = CreateGameObjectFromPrefab(CHARACTER_PREFAB_ASSET).GetComponent<ICharacter>();
                enemy.Element =
                    AssetDatabase.LoadAssetAtPath<Elemental>(
                        "Assets/ScriptableObjects/Character/Attributes/Elemental/Water/Water.asset");
                enemy.Init();
                hero.Attributes.TryGetAttributeValue(AttributeSets.Health, out var enemyHealth);
                Assert.AreEqual(100f, enemyHealth.CurrentValue);

                enemy.Attack(hero);

                hero.Attributes.TryGetAttributeValue(AttributeSets.Health, out var newHeroHealth);
                Assert.Less(newHeroHealth.CurrentValue, heroHealth.CurrentValue);
            }
        }
    }
}
using CryptoQuest.Battle;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay;
using NUnit.Framework;
using UnityEditor;

namespace CryptoQuest.Tests.Runtime.Battle
{
    [TestFixture]
    public class NormalAttackCommandTests : FixtureBase
    {
        private const string CHARACTER_PREFAB_ASSET = "Assets/Prefabs/Battle/Character.prefab";
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var attributeSet = AssetDatabase
                .LoadAllAssetsAtPath("Assets/ScriptableObjects/Character/Attributes/AttributeSets.asset")[0];
        }
        
        [Test]
        public void Execute_EnemyAttackHero_HeroHealthShouldLessThan100()
        {
            var hero = CreateGameObjectFromPrefab(CHARACTER_PREFAB_ASSET).GetComponent<ICharacter>();
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

            var command = new NormalAttackCommand(enemy, hero);
            command.Execute();

            hero.Attributes.TryGetAttributeValue(AttributeSets.Health, out var newHeroHealth);
            Assert.Less(newHeroHealth.CurrentValue, heroHealth.CurrentValue);
        }
    }
}
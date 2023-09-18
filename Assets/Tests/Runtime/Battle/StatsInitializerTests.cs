using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Tests.Runtime.Battle.Builder;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using NUnit.Framework;

namespace CryptoQuest.Tests.Runtime.Battle
{
    [TestFixture]
    public class StatsInitializerTests : BattleFixtureBase
    {
        [Test]
        public void SetStats_ShouldUpdateStats()
        {
            var characterGameObject = CreateCharacterFromPrefab();
            var character = A.Character.WithPrefab(characterGameObject).Build();

            IStatsInitializer statsInitializer = characterGameObject.GetComponent<IStatsInitializer>();
            Assert.NotNull(statsInitializer);

            statsInitializer.SetStats(new AttributeWithValue[]
            {
                new(AttributeSets.MaxHealth, 100f),
                new(AttributeSets.Health, 100f),
                new(AttributeSets.Strength, 50f),
            });

            character.Init();

            character.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var health);
            Assert.AreEqual(100f, health.CurrentValue);

            // We set strength to 50 in the prefab, but atk is calculated from strength, so it should be 50 too.
            character.AttributeSystem.TryGetAttributeValue(AttributeSets.Attack, out var atk);
            Assert.AreEqual(50f, atk.CurrentValue);
        }
    }
}
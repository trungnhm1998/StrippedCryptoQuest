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
        public void AttackShouldBe150()
        {
            var characterBuilder = A.Character;
            var character = characterBuilder
                .WithStats(new AttributeWithValue[]
                {
                    new(AttributeSets.MaxHealth, 200f),
                    new(AttributeSets.Health, 200f),
                    new(AttributeSets.Strength, 150f),
                    new(AttributeSets.Attack, 0f),
                })
                .Build();

            character.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var health);
            Assert.AreEqual(200f, health.CurrentValue);

            // We set strength to 150 in the prefab, but atk is calculated from strength, so it should be 150 too.
            character.AttributeSystem.TryGetAttributeValue(AttributeSets.Attack, out var atk);
            Assert.AreEqual(150f, atk.CurrentValue);
        }
    }
}
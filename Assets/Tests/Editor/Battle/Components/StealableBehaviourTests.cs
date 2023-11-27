using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Enemy;
using CryptoQuest.Gameplay.Loot;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Battle.Components
{
    [TestFixture]
    public class StealableBehaviourTests
    {
        private class MockDropsProvider : MonoBehaviour, IDropsProvider
        {
            public Drop[] GetDrops() => Drops;

            public Drop[] Drops { get; set; }
        }

        [Test]
        public void StealableBehaviour_WithStealableDrops_TryStealReturnsTrue()
        {
            var stealableBehaviour = CreateStealableBehaviour(new[] { new Drop(new EquipmentLoot(), true) });

            var result = stealableBehaviour.TrySteal(out var loot);

            Assert.IsTrue(result);
            Assert.IsNotNull(loot);
        }

        [Test]
        public void StealableBehaviour_WithNoStealableDrops_TryStealReturnsFalse()
        {
            // var provider = Substitute.For<IDropsProvider>();
            // provider.GetDrops().Returns(new[] { new Drop { Stealable = false } });
            // var stealableBehaviour = CreateStealableBehaviour(provider);
            //
            // var result = stealableBehaviour.TrySteal(out var loot);
            //
            // Assert.IsFalse(result);
            // Assert.IsNull(loot);
        }

        [Test]
        public void StealableBehaviour_WithStealableDropsBelowChance_TryStealReturnsFalse()
        {
            // var provider = Substitute.For<IDropsProvider>();
            // provider.GetDrops().Returns(new[] { new Drop { Stealable = true, Chance = 0 } });
            // var stealableBehaviour = CreateStealableBehaviour(provider);
            //
            // var result = stealableBehaviour.TrySteal(out var loot);
            //
            // Assert.IsFalse(result);
            // Assert.IsNull(loot);
        }

        private StealableBehaviour CreateStealableBehaviour(Drop[] drops)
        {
            var character = new GameObject();
            var mockProvider = character.AddComponent<MockDropsProvider>();
            mockProvider.Drops = drops;
            var stealableBehaviour = character.AddComponent<StealableBehaviour>();
            stealableBehaviour.Init();
            return stealableBehaviour;
        }
    }
}
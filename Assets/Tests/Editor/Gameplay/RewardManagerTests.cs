using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Gameplay
{
    [TestFixture]
    public class RewardManagerTests
    {
        [Test]
        public void MergeLoots_TwoCurrenciesLoot_OneCurrencyLootWithCorrectAmount()
        {
            var rewardSystem = new GameObject().AddComponent<RewardManager>();

            var currency = new CurrencyInfo(ScriptableObject.CreateInstance<CurrencySO>(), 1);
            var loots = new[]
            {
                new CurrencyLootInfo(currency),
                new CurrencyLootInfo(currency)
            };

            var mergedLoots = rewardSystem.MergeLoots(loots);

            Assert.AreEqual(1, mergedLoots.Length);
            Assert.AreEqual(2, ((CurrencyLootInfo)mergedLoots[0]).Item.Amount);
        }

        [Test]
        public void MergeLoots_TwoEquipments_ReturnTwoEquipments()
        {
            var rewardSystem = new GameObject().AddComponent<RewardManager>();

            var equipment = new EquipmentInfo(ScriptableObject.CreateInstance<EquipmentSO>());
            var loots = new[]
            {
                new EquipmentLootInfo(equipment),
                new EquipmentLootInfo(equipment)
            };

            var mergedLoots = rewardSystem.MergeLoots(loots);

            Assert.AreEqual(2, mergedLoots.Length);
        }

        [Test]
        public void MergeLoots_3Consumables_OneConsumableWithCorrectQuantity()
        {
            var rewardSystem = new GameObject().AddComponent<RewardManager>();

            var consumable = new UsableInfo(ScriptableObject.CreateInstance<UsableSO>());
            var loots = new[]
            {
                new UsableLootInfo(consumable),
                new UsableLootInfo(consumable),
                new UsableLootInfo(consumable)
            };

            var mergedLoots = rewardSystem.MergeLoots(loots);

            Assert.AreEqual(1, mergedLoots.Length);
        }
    }
}
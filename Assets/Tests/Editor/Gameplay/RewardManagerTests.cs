using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
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
            var currency = new CurrencyInfo(ScriptableObject.CreateInstance<CurrencySO>(), 1);
            var loots = new List<LootInfo>()
            {
                new CurrencyLootInfo(currency),
                new CurrencyLootInfo(currency)
            };

            var mergedLoots = RewardManager.CloneAndMergeLoots(loots);

            Assert.AreEqual(1, mergedLoots.Count);
            Assert.AreEqual(2, ((CurrencyLootInfo)mergedLoots[0]).Item.Amount);
        }

        [TestCase(new[] { 2f, 3f }, 5f)]
        [TestCase(new[] { 10f, 3f }, 13f)]
        [TestCase(new[] { 1f, 1f, 1f }, 3f)]
        public void MergeLoots_CurrenciesLoots_OneCurrencyLootWithCorrectAmount(float[] amount, float expected)
        {
            var loots = new LootInfo[amount.Length];
            var currencySO = ScriptableObject.CreateInstance<CurrencySO>();
            for (var index = 0; index < amount.Length; index++)
            {
                var f = amount[index];
                var currency = new CurrencyInfo(currencySO, f);
                loots[index] = new CurrencyLootInfo(currency);
            }

            var mergedLoots = RewardManager.CloneAndMergeLoots(loots.ToList());

            Assert.AreEqual(1, mergedLoots.Count);
            Assert.AreEqual(expected, ((CurrencyLootInfo)mergedLoots[0]).Item.Amount);
        }

        [TestCase(1f, 1f, 2f)]
        public void MergeLoots_TwoExpLoot_OneExpLootWithSumOfTwo(float first, float second, float expected)
        {
            var firstExpLoot = new ExpLoot(first); // after the merged loots, this should not be modified
            var loots = new List<LootInfo>()
            {
                firstExpLoot,
                new ExpLoot(second)
            };

            var mergedLoots = RewardManager.CloneAndMergeLoots(loots);

            Assert.AreNotEqual(firstExpLoot, mergedLoots[0]); // should be cloned
            Assert.AreEqual(1, mergedLoots.Count);
            Assert.AreEqual(expected, ((ExpLoot)mergedLoots[0]).Exp);
        }

        [Test]
        public void MergeLoots_TwoEquipments_ReturnTwoEquipments()
        {
            var equipment = new EquipmentInfo();
            var loots = new List<LootInfo>()
            {
                new EquipmentLootInfo(equipment),
                new EquipmentLootInfo(equipment)
            };

            var mergedLoots = RewardManager.CloneAndMergeLoots(loots);

            Assert.AreEqual(2, mergedLoots.Count);
        }

        [Test]
        public void MergeLoots_3Consumables_OneConsumableWithCorrectQuantity()
        {
            var baseItemSO = ScriptableObject.CreateInstance<ConsumableSO>();
            var consumable = new ConsumableInfo(baseItemSO);
            var loots = new List<LootInfo>()
            {
                new UsableLootInfo(consumable),
                new UsableLootInfo(consumable),
                new UsableLootInfo(consumable)
            };

            var mergedLoots = RewardManager.CloneAndMergeLoots(loots);

            Assert.AreEqual(1, mergedLoots.Count);
        }

        [Test]
        public void MergeLoots_ShouldNotModifyOriginalItemInList()
        {
            var currency = new CurrencyInfo(ScriptableObject.CreateInstance<CurrencySO>(), 1);
            var loots = new List<LootInfo>()
            {
                new CurrencyLootInfo(currency),
                new CurrencyLootInfo(currency)
            };

            var mergedLoots = RewardManager.CloneAndMergeLoots(loots);

            Assert.AreNotEqual(loots[0], mergedLoots[0]);
            Assert.AreNotEqual(((CurrencyLootInfo)loots[0]).Item, ((CurrencyLootInfo)mergedLoots[0]).Item);
        }
    }
}
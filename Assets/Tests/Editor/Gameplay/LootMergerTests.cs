using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Item;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Gameplay
{
    [TestFixture]
    public class LootMergerTests
    {
        private ILootMerger _lootMerger;

        [SetUp]
        public void Setup()
        {
            _lootMerger = new LootMerger();
        }

        [Test]
        public void Merge_TwoCurrenciesLoot_OneCurrencyLootWithCorrectAmount()
        {
            var currencySO = ScriptableObject.CreateInstance<CurrencySO>();
            var currencyLootInfo = new CurrencyLootInfo(new CurrencyInfo(currencySO, 1));
            var loots = new List<LootInfo>()
            {
                currencyLootInfo,
                new CurrencyLootInfo(new CurrencyInfo(currencySO, 1))
            };

            var mergedLoots = _lootMerger.Merge(loots);

            Assert.AreEqual(1, mergedLoots.Count);
            Assert.AreEqual(2, currencyLootInfo.Item.Amount);
        }

        [TestCase(new[] { 2f, 3f }, 5f)]
        [TestCase(new[] { 10f, 3f }, 13f)]
        [TestCase(new[] { 1f, 1f, 1f }, 3f)]
        public void Merge_CurrenciesLoots_OneCurrencyLootWithCorrectAmount(float[] amount, float expected)
        {
            var loots = new LootInfo[amount.Length];
            var currencySO = ScriptableObject.CreateInstance<CurrencySO>();
            for (var index = 0; index < amount.Length; index++)
            {
                var f = amount[index];
                var currency = new CurrencyInfo(currencySO, f);
                loots[index] = new CurrencyLootInfo(currency);
            }

            var mergedLoots = _lootMerger.Merge(loots.ToList());

            Assert.AreEqual(1, mergedLoots.Count);
            Assert.AreEqual(expected, ((CurrencyLootInfo)mergedLoots[0]).Item.Amount);
        }

        [TestCase(1f, 1f, 2f)]
        public void Merge_TwoExpLoot_OneExpLootWithSumOfTwo(float first, float second, float expected)
        {
            var firstExpLoot = new ExpLoot(first); // after the merged loots, this should not be modified
            var loots = new List<LootInfo>()
            {
                firstExpLoot,
                new ExpLoot(second)
            };

            var mergedLoots = _lootMerger.Merge(loots);

            Assert.AreEqual(1, mergedLoots.Count);
            Assert.AreEqual(expected, firstExpLoot.Exp);
        }

        [Test]
        public void Merge_3Consumables_OneConsumableWithCorrectQuantity()
        {
            var baseItemSO = ScriptableObject.CreateInstance<ConsumableSO>();
            var firstConsumableLoot =
                new ConsumableLootInfo(
                    new ConsumableInfo(baseItemSO)); // after the merged loots, this should not be modified
            var loots = new List<LootInfo>()
            {
                firstConsumableLoot,
                new ConsumableLootInfo(new ConsumableInfo(baseItemSO)),
                new ConsumableLootInfo(new ConsumableInfo(baseItemSO))
            };

            var mergedLoots = _lootMerger.Merge(loots);

            Assert.AreEqual(1, mergedLoots.Count);
            Assert.AreEqual(3, firstConsumableLoot.Item.Quantity);
        }

        [Test]
        public void Merge_FiveConsumablesOfTwo_ShouldReturnTwoLootsWithCorrectSum()
        {
            var first = ScriptableObject.CreateInstance<ConsumableSO>();
            var second = ScriptableObject.CreateInstance<ConsumableSO>();
            var firstConsumableLoot =
                new ConsumableLootInfo(
                    new ConsumableInfo(first)); // after the merged loots, this should not be modified
            var secondConsumableLootInfo = new ConsumableLootInfo(new ConsumableInfo(second));
            var loots = new List<LootInfo>()
            {
                firstConsumableLoot,
                new ConsumableLootInfo(new ConsumableInfo(first)),
                secondConsumableLootInfo,
                new ConsumableLootInfo(new ConsumableInfo(first)),
                new ConsumableLootInfo(new ConsumableInfo(second))
            };

            var mergedLoots = _lootMerger.Merge(loots);

            Assert.AreEqual(2, mergedLoots.Count);
            Assert.AreEqual(3, firstConsumableLoot.Item.Quantity);
            Assert.AreEqual(2, secondConsumableLootInfo.Item.Quantity);
        }
    }
}
using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;

namespace CryptoQuest.Battle
{
    public class LootMerger : ILootMerger
    {
        public List<LootInfo> Merge(List<LootInfo> loots)
        {
            var mergedLoots = new List<LootInfo>(loots);

            var i = 0;
            while (i < mergedLoots.Count)
            {
                var loot = mergedLoots[i];
                if (loot.IsValid() == false)
                {
                    i++;
                    continue;
                }

                loot.TryMerge(this);
                for (int j = i + 1; j < mergedLoots.Count;)
                {
                    var otherLoot = mergedLoots[j];
                    if (otherLoot.IsValid() && otherLoot.TryMerge(this))
                    {
                        mergedLoots.RemoveAt(j);
                        continue;
                    }

                    j++;
                }

                i++;
                _mergingConsumable = null;
                _mergingCurrency = null;
                _mergingExp = null;
                _mergingStone = null;
            }

            return mergedLoots;
        }

        #region Consumable

        private ConsumableLootInfo _mergingConsumable;

        // TODO: There is an edge case where _mergingLoot and otherLoot is the same and cause double quantity
        public bool Merge(ConsumableLootInfo otherLoot)
        {
            if (_mergingConsumable == null)
            {
                _mergingConsumable = otherLoot;
                return false;
            }

            if (_mergingConsumable.Item.Data != otherLoot.Item.Data) return false;

            _mergingConsumable.Item.SetQuantity(_mergingConsumable.Item.Quantity + otherLoot.Item.Quantity);
            return true;
        }

        #endregion

        #region Exp

        private ExpLoot _mergingExp;

        public bool Merge(ExpLoot otherLoot)
        {
            if (_mergingExp == null)
            {
                _mergingExp = otherLoot;
                return false;
            }

            _mergingExp.Exp += otherLoot.Exp;
            return true;
        }

        #endregion

        #region Currency

        private CurrencyLootInfo _mergingCurrency;

        public bool Merge(CurrencyLootInfo loot)
        {
            if (_mergingCurrency == null)
            {
                _mergingCurrency = loot;
                return false;
            }

            if (_mergingCurrency.Item.Data != loot.Item.Data) return false;

            _mergingCurrency.Item.UpdateCurrencyAmount(loot.Item.Amount);
            return true;
        }

        #endregion

        #region Magic Stone

        private MagicStoneLoot _mergingStone;

        public bool Merge(MagicStoneLoot otherLoot)
        {
            if (_mergingStone == null)
            {
                _mergingStone = otherLoot;
                return false;
            }

            if (_mergingStone.StoneId != otherLoot.StoneId) return false;

            _mergingStone.Quantity += otherLoot.Quantity;
            return true;
        }

        #endregion
    }
}
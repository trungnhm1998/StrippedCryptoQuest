using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;

namespace CryptoQuest.Gameplay.Reward
{
    public interface IRewardMerger
    {
        bool Visit(ExpLoot loot);
        bool Visit(CurrencyLootInfo loot);
        bool Visit(UsableLootInfo loot);
        bool Merge(ExpLoot loot);
        bool Merge(CurrencyLootInfo loot);
        bool Merge(UsableLootInfo loot);
    }

    /// <summary>
    /// Merge multiple loots of the same type into one
    ///
    /// Using the visitor pattern to avoid using reflection, but triple dispatch instead
    /// </summary>
    public class BasicMerger : IRewardMerger
    {
        private List<LootInfo> _loots;

        public BasicMerger(ref List<LootInfo> loots)
        {
            _loots = loots;
        }

        #region XP

        private ExpLoot _mergingExpLoot;

        public bool Visit(ExpLoot expLoot)
        {
            _mergingExpLoot = expLoot;
            return TryMergeWithOtherLoots(expLoot);
        }

        public bool Merge(ExpLoot expLoot)
        {
            if (_mergingExpLoot == null || _mergingExpLoot.IsValid() == false) return false;
            _mergingExpLoot.Merge(expLoot);
            return true;
        }

        #endregion

        #region Currency

        private CurrencyLootInfo _mergingCurrencyLoot;

        public bool Visit(CurrencyLootInfo loot)
        {
            if (loot.IsValid() == false) return false;
            _mergingCurrencyLoot = loot;
            return TryMergeWithOtherLoots(loot);
        }

        public bool Merge(CurrencyLootInfo loot)
        {
            if (_mergingCurrencyLoot == null || _mergingCurrencyLoot.IsValid() == false) return false;
            if (loot.IsValid() == false) return false;
            _mergingCurrencyLoot.Merge(loot);
            return true;
        }

        #endregion

        #region Consumable

        private UsableLootInfo _mergingConsumable;

        public bool Visit(UsableLootInfo loot)
        {
            if (loot == null || loot.IsValid() == false) return false;
            _mergingConsumable = loot;
            return TryMergeWithOtherLoots(loot);
        }

        public bool Merge(UsableLootInfo loot)
        {
            if (_mergingConsumable == null || _mergingConsumable.IsValid() == false) return false;
            if (loot == null || loot.IsValid() == false || _mergingConsumable.Item.Data != loot.Item.Data) return false;
            _mergingConsumable.Merge(loot);
            return true;
        }

        #endregion

        private bool TryMergeWithOtherLoots(LootInfo loot)
        {
            var currentMergingIndex = _loots.IndexOf(loot);
            if (currentMergingIndex == -1) return false;
            for (int i = currentMergingIndex + 1; i < _loots.Count;)
            {
                var nextLoot = _loots[i];
                if (nextLoot.Merge(this))
                {
                    _loots[currentMergingIndex] = loot;
                    _loots.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            return true;
        }
    }
}
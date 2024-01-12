using System;
using CryptoQuest.Battle;
using CryptoQuest.Inventory;
using CryptoQuest.UI.Dialogs.RewardDialog;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class MagicStoneLoot : LootInfo
    {
        [SerializeField] private string _stoneId;
        public string StoneId => _stoneId;

        public int Quantity;
        public override bool IsItem => true;
        public MagicStoneLoot() { }

        public MagicStoneLoot(string stoneId, int quantity)
        {
            Quantity = quantity;
            _stoneId = stoneId;
        }

        public override LootInfo Clone() => new MagicStoneLoot(_stoneId, Quantity);

        public override bool IsValid() => Quantity > 0 && !string.IsNullOrEmpty(_stoneId);

        public override void Accept(ILootVisitor lootController) => lootController.Visit(this);

        public override bool TryMerge(ILootMerger lootMerger) => lootMerger.Merge(this);

        public override void Accept(UIRewardItem rewardUI) => rewardUI.Visit(this);
    }
}
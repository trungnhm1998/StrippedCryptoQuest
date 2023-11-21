using System;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Reward;
using CryptoQuest.UI.Dialogs.RewardDialog;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class ExpLoot : LootInfo
    {
        [field: SerializeField] public float Exp { get; private set; }
        public ExpLoot(float experiencePoints) => Exp = experiencePoints;

        public override void AddItemToInventory(IInventoryController _) => RewardManager.RewardPlayerExp(Exp);

        public override UI.Dialogs.RewardDialog.Reward CreateRewardUI()
            => new GenericReward($"{Exp} EXP");

        public override LootInfo Clone() => new ExpLoot(Exp);
        public override bool AcceptMerger(IRewardMerger merger) => merger.Visit(this);
        public override bool Merge(IRewardMerger merger) => merger.Merge(this);
        public override bool IsValid() => Exp > 0;
        
        public void Merge(ExpLoot loot) => Exp += loot.Exp;
    }
}
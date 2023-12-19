using System;
using CryptoQuest.Battle;
using CryptoQuest.Inventory;
using CryptoQuest.UI.Dialogs.RewardDialog;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class ExpLoot : LootInfo
    {
        [field: SerializeField] public float Exp { get; set; } = 1f;
        public ExpLoot() { }
        public ExpLoot(float experiencePoints) => Exp = experiencePoints;
        public override bool IsItem => false;
        public override LootInfo Clone() => new ExpLoot(Exp);
        public override bool IsValid() => Exp > 0;
        public override void Accept(ILootVisitor lootController) => lootController.Visit(this);
        public override bool TryMerge(ILootMerger lootMerger) => lootMerger.Merge(this);
        public override void Accept(UIRewardItem rewardUI) => rewardUI.Visit(this);
    }
}
using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
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

        public override void AddItemToInventory(InventorySO inventory)
        {
            RewardManager.RewardPlayerExp(Exp);
        }

        public override UI.Dialogs.RewardDialog.Reward CreateRewardUI()
            => new GenericReward($"{Exp} EXP");

        public override LootInfo Clone() => new ExpLoot(Exp);
        public override bool Merge(LootInfo otherLoot)
        {
            if (otherLoot is not ExpLoot expLoot) return false;
            Exp += expLoot.Exp;
            return true;
        }
    }
}
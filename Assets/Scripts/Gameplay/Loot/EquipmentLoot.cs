using System;
using CryptoQuest.Battle;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.UI.Dialogs.RewardDialog;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class EquipmentLoot : LootInfo
    {
        [SerializeField] private string _equipmentId;

        public override bool IsItem => true;

        public string EquipmentId => _equipmentId;

        public override LootInfo Clone()
        {
            var loot = new EquipmentLoot
            {
                _equipmentId = _equipmentId
            };
            return loot;
        }

        public override bool IsValid() => string.IsNullOrEmpty(_equipmentId);

        public override void Accept(ILootVisitor lootController) => lootController.Visit(this);
        public override bool TryMerge(ILootMerger lootMerger) => false;
        public override void Accept(UIRewardItem rewardUI) => rewardUI.Visit(this);
        
    }
}
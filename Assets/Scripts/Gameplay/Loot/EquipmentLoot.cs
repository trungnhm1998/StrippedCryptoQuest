using System;
using CryptoQuest.Battle;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Dialogs.RewardDialog;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class EquipmentLoot : LootInfo
    {
        [SerializeField] private EquipmentSO _equipmentSO;
        public EquipmentSO EquipmentSO => _equipmentSO;

        public override bool IsItem => true;

        public override LootInfo Clone()
        {
            var loot = new EquipmentLoot
            {
                _equipmentSO = _equipmentSO
            };
            return loot;
        }

        public override bool IsValid() => _equipmentSO != null;

        public override void Accept(ILootVisitor lootController) => lootController.Visit(this);
        public override bool TryMerge(ILootMerger lootMerger) => false;
        public override void Accept(UIRewardItem rewardUI) => rewardUI.Visit(this);
        
    }
}
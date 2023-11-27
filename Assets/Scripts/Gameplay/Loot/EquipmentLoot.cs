using System;
using CryptoQuest.Battle;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Item.Equipment;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class EquipmentLoot : LootInfo
    {
        [SerializeField] private EquipmentSO _equipmentSO;
        public EquipmentSO EquipmentSO => _equipmentSO;

        public override UI.Dialogs.RewardDialog.Reward CreateRewardUI()
        {
            throw new NotImplementedException();
        }

        public override LootInfo Clone()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid() => _equipmentSO != null;

        public override void Accept(ILootVisitor lootController) => lootController.Visit(this);
        public override bool TryMerge(ILootMerger lootMerger) => false;
    }
}
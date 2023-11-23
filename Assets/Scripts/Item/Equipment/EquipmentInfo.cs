using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Item.Equipment
{
    [Serializable]
    public class EquipmentInfo : ItemInfo, IEquatable<EquipmentInfo>
    {
        [field: SerializeField] public int Level { get; set; } = 1;
        [SerializeField] private EquipmentData _data;

        public EquipmentData Data
        {
            get => _data;
            set => _data = value;
        }

        public AttributeWithValue[] Stats => Data.Stats;
        public bool IsNftItem => Id > 0;
        public RaritySO Rarity => Data.Rarity;
        public float ValuePerLvl => Data.ValuePerLvl;

        public override int Price => 0;
        public override int SellPrice => 0;

        public PassiveAbility[] Passives => Data.Passives;
        public EquipmentPrefab Config { get; set; }
        public LocalizedString DisplayName => Config.DisplayName;
        public EquipmentTypeSO EquipmentType => Config.EquipmentType;
        public EquipmentSlot.EType[] AllowedSlots => Config.AllowedSlots;
        public EquipmentSlot.EType[] RequiredSlots => Config.RequiredSlots;

        public EquipmentInfo() => Level = 1;

        public EquipmentInfo(uint id) : base(id)
        {
            Level = 1;
        }

        #region Utils

        public static bool operator ==(EquipmentInfo left, EquipmentInfo right)
        {
            if (left is null) return right is null;
            return Equals(left, right);
        }

        public bool Equals(EquipmentInfo other) => other != null && ReferenceEquals(this, other);

        public static bool operator !=(EquipmentInfo left, EquipmentInfo right) => !(left == right);

        public override bool Equals(object obj) => Equals(obj as EquipmentInfo);

        public override int GetHashCode() => (Id, Data.ID, Data.PrefabId).GetHashCode();

        public override ItemInfo Clone() =>
            new EquipmentInfo()
            {
                Id = Id,
                Level = Level,
                Data = Data,
            };

        #endregion

        public override bool IsValid()
            => Data != null && !string.IsNullOrEmpty(Data.ID) && !string.IsNullOrEmpty(Data.PrefabId);

        public virtual bool ContainedInInventory(IInventoryController inventoryController) =>
            inventoryController.Contains(this);

        public override bool AddToInventory(IInventoryController inventoryController) => inventoryController.Add(this);

        public override bool RemoveFromInventory(IInventoryController inventoryController) =>
            inventoryController.Remove(this);
    }

    [Serializable]
    public class NftEquipment : EquipmentInfo
    {
        [field: SerializeField] public string TokenId { get; set; }
        public NftEquipment(uint equipmentResponseID) : base(equipmentResponseID) { }

        public override bool AddToInventory(IInventoryController inventoryController) => inventoryController.Add(this);

        public override bool RemoveFromInventory(IInventoryController inventoryController) =>
            inventoryController.Remove(this);

        public override bool ContainedInInventory(IInventoryController inventoryController) =>
            inventoryController.Contains(this);
    }
}
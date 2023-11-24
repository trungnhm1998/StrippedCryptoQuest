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
    public abstract class EquipmentInfo : ItemInfo, IEquatable<EquipmentInfo>
    {
        [field: SerializeField] public int Level { get; set; } = 1;

        public abstract EquipmentData Data { get; }
        public AttributeWithValue[] Stats => Data.Stats;
        public RaritySO Rarity => Data.Rarity;
        public float ValuePerLvl => Data.ValuePerLvl;
        public PassiveAbility[] Passives => Data.Passives;
        public EquipmentPrefab Config { get; set; }
        public LocalizedString DisplayName => Config.DisplayName;
        public EquipmentTypeSO EquipmentType => Config.EquipmentType;
        public EquipmentSlot.EType[] AllowedSlots => Config.AllowedSlots;
        public EquipmentSlot.EType[] RequiredSlots => Config.RequiredSlots;
        public abstract bool IsNftItem { get; }

        public EquipmentInfo() => Level = 1;

        #region Utils

        public static bool operator ==(EquipmentInfo left, EquipmentInfo right)
        {
            if (left is null) return right is null;
            return Equals(left, right);
        }

        public bool Equals(EquipmentInfo other) => other != null && ReferenceEquals(this, other);

        public static bool operator !=(EquipmentInfo left, EquipmentInfo right) => !(left == right);

        public override bool Equals(object obj) => Equals(obj as EquipmentInfo);

        public override int GetHashCode() => (Data.ID, Data.PrefabId).GetHashCode();

        #endregion

        public override bool IsValid()
            => Data != null && !string.IsNullOrEmpty(Data.ID) && !string.IsNullOrEmpty(Data.PrefabId);

        public abstract bool ContainedInInventory(IInventoryController inventoryController);
    }
}
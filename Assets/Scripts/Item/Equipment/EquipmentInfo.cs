using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Gameplay.Inventory;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Item.Equipment
{
    [Serializable]
    public abstract class EquipmentInfo : IEquipment, IEquatable<EquipmentInfo>
    {
        [field: SerializeField] public uint Id { get; set; }
        [field: SerializeField] public int Level { get; set; } = 1;
        [field: SerializeField] public EquipmentData Data { get; set; }
        public AttributeWithValue[] Stats => Data.Stats;
        public RaritySO Rarity => Data.Rarity;
        public float ValuePerLvl => Data.ValuePerLvl;
        public EquipmentPrefab Prefab => Data.Prefab;
        public EquipmentTypeSO Type => Data.Prefab.EquipmentType;
        public ESlot[] RequiredSlots => Data.Prefab.RequiredSlots;
        public ESlot[] AllowedSlots => Data.Prefab.AllowedSlots;

        public PassiveAbility[] Passives => Data.Passives;

        public abstract bool IsNft { get; }
        public LocalizedString DisplayName => Data.Prefab.DisplayName;

        #region Utils

        public static bool operator ==(EquipmentInfo left, EquipmentInfo right)
        {
            if (left is null) return right is null;
            return Equals(left, right);
        }

        public bool Equals(EquipmentInfo other) => other != null && ReferenceEquals(this, other);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EquipmentInfo)obj);
        }

        public static bool operator !=(EquipmentInfo left, EquipmentInfo right) => !(left == right);

        public virtual bool Equals(IEquipment other) => other is not null && ReferenceEquals(this, other);

        public override int GetHashCode() => HashCode.Combine(Id, Level, Data, IsNft);

        #endregion

        public virtual bool IsValid()
            => Data != null && !string.IsNullOrEmpty(Data.ID) && Data.Prefab != null;

        public abstract bool AddToInventory(IInventoryController inventory);
        public abstract bool RemoveFromInventory(IInventoryController inventory);
    }
}
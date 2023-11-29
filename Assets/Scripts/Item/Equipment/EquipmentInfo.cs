using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    public interface IEquipment
    {
        public uint Id { get; set; }
        public int Level { get; set; }
        public EquipmentData Data { get; set; }
        public AttributeWithValue[] Stats { get; }
        public RaritySO Rarity { get; }
        public float ValuePerLvl { get; }
        public EquipmentPrefab Prefab { get; }
        public EquipmentTypeSO Type { get; }
        public EquipmentSlot.EType[] RequiredSlots { get; }
        public EquipmentSlot.EType[] AllowedSlots { get; }

        public PassiveAbility[] Passives { get; }

        public bool IsNft { get; }

        public bool IsValid();

        public bool ContainedInInventory(IInventoryController inventoryController);
        public bool AddToInventory(IInventoryController inventory);
        public bool RemoveFromInventory(IInventoryController inventory);
    }

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
        public EquipmentSlot.EType[] RequiredSlots => Data.Prefab.RequiredSlots;
        public EquipmentSlot.EType[] AllowedSlots => Data.Prefab.AllowedSlots;

        public PassiveAbility[] Passives => Data.Passives;

        public abstract bool IsNft { get; }

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

        public abstract bool ContainedInInventory(IInventoryController inventoryController);
        public abstract bool AddToInventory(IInventoryController inventory);
        public abstract bool RemoveFromInventory(IInventoryController inventory);
    }
}
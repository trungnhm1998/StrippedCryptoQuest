using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    [Serializable]
    public abstract class EquipmentInfo : IEquatable<EquipmentInfo>
    {
        [field: SerializeField] public int Level { get; set; } = 1;

        public abstract EquipmentData Data { get; }

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

        public static bool operator !=(EquipmentInfo left, EquipmentInfo right) => !(left == right);

        public override bool Equals(object obj) => Equals(obj as EquipmentInfo);

        public override int GetHashCode() => (Data.ID, Data.Prefab).GetHashCode();

        #endregion

        public bool IsValid()
            => Data != null && !string.IsNullOrEmpty(Data.ID) && Data.Prefab != null;

        public abstract bool ContainedInInventory(IInventoryController inventoryController);
        public abstract bool AddToInventory(IInventoryController inventory);
        public abstract bool RemoveFromInventory(IInventoryController inventory);
    }
}
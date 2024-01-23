using System;
using CryptoQuest.AbilitySystem.Abilities;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Item.Equipment
{
    [Serializable]
    public class Equipment : IEquipment, IEquatable<Equipment>
    {
        [field: SerializeField] public int Id { get; set; }
        [field: SerializeField] public int Level { get; set; } = 1;
        [field: SerializeField] public bool IsNft { get; set; }
        [field: SerializeField] public EquipmentData Data { get; set; }
        [field: SerializeField] public int AttachCharacterId { get; set; } = -1;
        public AttributeWithValue[] Stats => Data.Stats;
        public RaritySO Rarity => Data.Rarity;
        public float ValuePerLvl => Data.ValuePerLvl;
        public EquipmentPrefab Prefab => Data.Prefab;
        public EquipmentTypeSO Type => Data.Prefab.EquipmentType;
        public ESlot[] RequiredSlots => Data.Prefab.RequiredSlots;
        public ESlot[] AllowedSlots => Data.Prefab.AllowedSlots;

        public PassiveAbility[] Passives => Data.Passives;
        public LocalizedString DisplayName => Data.Prefab.DisplayName;


        #region Utils

        public static bool operator ==(Equipment left, Equipment right)
        {
            if (left is null) return right is null;
            return Equals(left, right);
        }

        public bool Equals(Equipment other) => other != null && ReferenceEquals(this, other);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Equipment)obj);
        }

        public static bool operator !=(Equipment left, Equipment right) => !(left == right);

        public virtual bool Equals(IEquipment other) => other is not null && ReferenceEquals(this, other);

        public override int GetHashCode() => HashCode.Combine(Id, Level, Data, IsNft);

        #endregion

        public virtual bool IsEquipped() => AttachCharacterId >= 0;
        public virtual bool IsValid()
            => Data != null && !string.IsNullOrEmpty(Data.ID) && Data.Prefab != null;
    }
}
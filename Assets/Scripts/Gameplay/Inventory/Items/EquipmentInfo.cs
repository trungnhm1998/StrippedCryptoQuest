using System;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.Items
{
    [Serializable]
    public class EquipmentInfo : ItemInfo<EquipmentSO>, IEquatable<EquipmentInfo>
    {
        [field: SerializeField] public RaritySO Rarity { get; private set; }
        [field: SerializeField] public bool IsNftItem { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public StatsDef Stats { get; private set; }
        public EquipmentSlot.EType[] RequiredSlots => Data.RequiredSlots;

        public InfiniteEffectScriptableObject EffectDef { get; set; }

        /// <summary>
        /// The effect that being active when this equipment is equipped
        /// this contains all <see cref="ActiveEffectSpecification.ComputedModifiers"/> that this equipment give to the character
        /// </summary>
        private ActiveEffectSpecification _activeEffect = new();

        public EquipmentInfo() { }
        public EquipmentInfo(EquipmentSO data) : base(data) { }

        public ActiveEffectSpecification ActiveEffect => _activeEffect;

        public void SetActiveEffectSpec(ActiveEffectSpecification applyEquipmentEffect)
        {
            _activeEffect = applyEquipmentEffect;
        }

        /// <summary>
        /// Maybe this equipment have some kind of ability/passive skill?
        ///
        /// e.g.
        /// Something like while equip this item, you have 10% chance to do something
        /// When health is below 50%, your stats boost by 10%
        /// </summary>
        protected void Activate() { }

        #region Utils

        public static bool operator ==(EquipmentInfo left, EquipmentInfo right)
        {
            if (left is null) return right is null;
            return Equals(left, right);
        }

        public bool Equals(EquipmentInfo other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public static bool operator !=(EquipmentInfo left, EquipmentInfo right) => !(left == right);

        public override bool Equals(object obj) => Equals(obj as EquipmentInfo);

        public override int GetHashCode() => (Id, Data).GetHashCode();

        public EquipmentInfo Clone()
        {
            return new EquipmentInfo(Data)
            {
                Id = Id,
                Level = Level,
                Stats = Stats,
                EffectDef = EffectDef,
            };
        }

        #endregion

        public bool IsCompatibleWithCharacter(CharacterSpec inspectingCharacter)
        {
            if (!IsValid()) return false;

            CharacterClass[] equipmentAllowedClasses = Data.EquipmentType.AllowedClasses;
            CharacterClass characterClass = inspectingCharacter.Class;

            if (Data.RequiredCharacterLevel > inspectingCharacter.Level)
            {
                Debug.LogWarning("Character level is not enough");
                return false;
            }

            if (equipmentAllowedClasses.Length <= 0)
            {
                Debug.LogWarning("Equipment allowed classes is null or empty");
                return false;
            }

            if (!Array.Exists(equipmentAllowedClasses, allowedClass => allowedClass == characterClass))
            {
                Debug.LogWarning("Character class is not allowed");
                return false;
            }

            return true;
        }

        public override bool IsValid()
        {
            return base.IsValid() && Data.EquipmentType != null;
        }

#if UNITY_EDITOR

        /// <summary>
        /// This method is used to set rarity of equipment
        /// </summary>
        /// <param name="rarity"></param>
        public void Editor_SetRarity(RaritySO rarity)
        {
            Rarity = rarity;
        }

        /// <summary>
        /// This method is used to set level of equipment
        /// </summary>
        /// <param name="isNftItem"></param>
        public void Editor_SetIsNftItem(bool isNftItem)
        {
            IsNftItem = isNftItem;
        }

        /// <summary>
        /// This method is used to set level of equipment
        /// </summary>
        /// <param name="stats"></param>
        public void Editor_SetStats(StatsDef stats)
        {
            Stats = stats;
        }

#endif
    }
}
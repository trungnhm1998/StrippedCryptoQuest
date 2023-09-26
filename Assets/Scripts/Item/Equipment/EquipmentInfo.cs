using System;
using CryptoQuest.Gameplay.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    [Serializable]
    public class EquipmentInfo : ItemInfo, IEquatable<EquipmentInfo>
    {
        [SerializeField] private string _definitionId;
        public string DefinitionId => _definitionId;

        [field: SerializeField] public int Level { get; private set; }
        public EquipmentPrefab Data => Prefab;
        public AttributeWithValue[] Stats => Def.Stats;
        public EquipmentSlot.EType[] RequiredSlots => Prefab.RequiredSlots;

        public GameplayEffectDefinition EffectDef { get; set; }

        /// <summary>
        /// The effect that being active when this equipment is equipped
        /// this contains all <see cref="ActiveEffectSpecification.ComputedModifiers"/> that this equipment give to the character
        /// </summary>
        private ActiveEffectSpecification _activeEffect = new();

        public ActiveEffectSpecification ActiveEffect => _activeEffect;
        public bool IsNftItem => Def.IsNft;
        public RaritySO Rarity => Def.Rarity;
        public float ValuePerLvl => Def.ValuePerLvl;
        public EquipmentDef Def { get; set; }
        public EquipmentPrefab Prefab { get; set; }

        public override int Price => Def.Price;
        public override int SellPrice => Def.SellPrice;

        public EquipmentInfo()
        {
            Level = 1;
        }

        public EquipmentInfo(string definitionId, int lvl = 1)
        {
            _definitionId = definitionId;
            Level = lvl;
        }


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

        public override int GetHashCode() => (Id, Def.ID).GetHashCode();

        public EquipmentInfo Clone()
        {
            return new EquipmentInfo(DefinitionId)
            {
                Id = Id,
                Level = Level,
                EffectDef = EffectDef,
            };
        }

        #endregion

        public bool IsCompatibleWithCharacter(CharacterSpec inspectingCharacter)
        {
            if (!IsValid()) return false;

            CharacterClass[] equipmentAllowedClasses = Prefab.EquipmentType.AllowedClasses;
            CharacterClass characterClass = inspectingCharacter.Class;

            if (Prefab.RequiredCharacterLevel > inspectingCharacter.Level)
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
            return Def != null && Prefab != null && Prefab.EquipmentType != null;
        }
    }
}
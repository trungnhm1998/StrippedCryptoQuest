using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character;
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

        [SerializeField] private int _level = 1;
        public int Level => _level;

        public EquipmentPrefab Data => Prefab;
        public AttributeWithValue[] Stats => Def.Stats;
        public EquipmentSlot.EType[] RequiredSlots => Prefab.RequiredSlots;
        public EquipmentSlot.EType[] AllowedSlots => Prefab.AllowedSlots;

        [field: NonSerialized] public GameplayEffectDefinition EffectDef { get; set; }

        /// <summary>
        /// The effect that being active when this equipment is equipped
        /// this contains all <see cref="ActiveGameplayEffect.ComputedModifiers"/> that this equipment give to the character
        /// </summary>
        [NonSerialized] private ActiveGameplayEffect _activeGameplayEffect = new();

        public ActiveGameplayEffect activeGameplayEffect => _activeGameplayEffect;
        public bool IsNftItem => Def.IsNft;
        public RaritySO Rarity => Def.Rarity;
        public float ValuePerLvl => Def.ValuePerLvl;
        [field: NonSerialized] public EquipmentDef Def { get; set; }
        [field: NonSerialized] public EquipmentPrefab Prefab { get; set; }

        public override int Price => Def.Price;
        public override int SellPrice => Def.SellPrice;

        public PassiveAbility[] Passives => Def.Passives;

        /// <summary>
        /// This is not using anywhere yet but
        /// I saved the equipped hero unit Id just in case
        /// </summary>
        [NonSerialized] private int _equippedHeroUnitId = 0;
        public bool IsEquipped => _equippedHeroUnitId != 0;

        public EquipmentInfo()
        {
            _level = 1;
        }

        public EquipmentInfo(uint id, string definitionId, int lvl)
        {
            Id = id;
            _definitionId = definitionId;
            _level = lvl;
        }

        public EquipmentInfo(string definitionId, int lvl = 1)
        {
            _definitionId = definitionId;
            _level = lvl;
        }


        public void SetActiveEffectSpec(ActiveGameplayEffect applyEquipmentGameplayEffect)
        {
            _activeGameplayEffect = applyEquipmentGameplayEffect;
        }

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
            return this.DefinitionId == other.DefinitionId && Id == other.Id;
        }

        public static bool operator !=(EquipmentInfo left, EquipmentInfo right) => !(left == right);

        public override bool Equals(object obj) => Equals(obj as EquipmentInfo);

        public override int GetHashCode() => (Id, Def.ID).GetHashCode();

        public EquipmentInfo Clone()
        {
            return new EquipmentInfo(DefinitionId)
            {
                Id = Id,
                _level = Level,
                Def = Def,
                Prefab = Prefab
            };
        }

        #endregion

        public bool IsCompatibleWithHero(HeroBehaviour hero)
        {
            if (!IsValid()) return false;

            CharacterClass[] equipmentAllowedClasses = Prefab.EquipmentType.AllowedClasses;
            CharacterClass characterClass = hero.Class;

            if (Def.RequiredCharacterLevel > hero.Level)
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
            return !string.IsNullOrEmpty(_definitionId);
        }

        public void ResetEquippedHeroUnitId()
        {
            _equippedHeroUnitId = 0;
        }

        public void SetEquippedHeroUnitId(int heroUnitId)
        {
            _equippedHeroUnitId = heroUnitId;
        }

        public bool Loaded() => Prefab != null && Prefab.EquipmentType != null && Def != null;

        public void SetLevel(int level) => _level = level;
    }
}
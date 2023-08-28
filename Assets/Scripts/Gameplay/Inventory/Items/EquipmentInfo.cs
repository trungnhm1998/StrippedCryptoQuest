using System;
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
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public StatsDef Stats { get; private set; }
        public EquipmentSlot.EType[] RequiredSlots => Data.RequiredSlots;

        public InfiniteEffectScriptableObject EffectDef { get; set; }

        /// <summary>
        /// The effect that being active when this equipment is equipped
        /// this contains all <see cref="ActiveEffectSpecification.ComputedModifiers"/> that this equipment give to the character
        /// </summary>
        private ActiveEffectSpecification _activeEffect = new();

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
        protected override void Activate() { }

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
            if (Data != other.Data) return false;
            return Id == other.Id;
        }

        public static bool operator !=(EquipmentInfo left, EquipmentInfo right) => !(left == right);

        public override bool Equals(object obj) => Equals(obj as EquipmentInfo);

        public override int GetHashCode() => (Id, Data).GetHashCode();

        #endregion
    }
}
using CryptoQuest.AbilitySystem.Abilities;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Item.Equipment
{
    [CreateAssetMenu(fileName = "Equipment", menuName = "Crypto Quest/Inventory/Equipment")]
    public class EquipmentSO : ScriptableObject, IEquipment
    {
        [field: SerializeField] public EquipmentData Data { get; set; }
        public int Id { get; set; }
        public int Level { get; set; }
        public bool IsNft => false;
        public AttributeWithValue[] Stats => Data.Stats;
        public RaritySO Rarity => Data.Rarity;
        public float ValuePerLvl => Data.ValuePerLvl;
        public EquipmentPrefab Prefab => Data.Prefab;
        public EquipmentTypeSO Type => Data.Prefab.EquipmentType;
        public ESlot[] RequiredSlots => Data.Prefab.RequiredSlots;
        public ESlot[] AllowedSlots => Data.Prefab.AllowedSlots;
        public PassiveAbility[] Passives => Data.Passives;
        public LocalizedString DisplayName => Data.Prefab.DisplayName;

        public int AttachCharacterId { get; set; }

        public bool IsEquipped() => false;
        public bool IsValid() => true;
    }
}
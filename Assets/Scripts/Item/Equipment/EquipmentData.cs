using System;
using CryptoQuest.AbilitySystem.Abilities;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    [Serializable]
    public class EquipmentData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public EquipmentPrefab Prefab { get; set; }
        [field: SerializeField] public RaritySO Rarity { get; set; }
        [field: SerializeField] public int Stars { get; set; }
        [field: SerializeField] public int RequiredCharacterLevel { get; set; }
        [field: SerializeField] public int MinLevel { get; set; }
        [field: SerializeField] public int MaxLevel { get; set; }
        [field: SerializeField] public float ValuePerLvl { get; set; }
        [field: SerializeField] public AttributeWithValue[] Stats { get; set; }
        [field: SerializeField] public PassiveAbility[] Passives { get; set; } = Array.Empty<PassiveAbility>();
        [field: SerializeField] public int StoneSlots { get; set; }
    }
}
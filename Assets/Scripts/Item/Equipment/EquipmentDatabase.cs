using System;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    [Serializable]
    public class EquipmentData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string PrefabId { get; set; }
        [field: SerializeField] public RaritySO Rarity { get; set; }
        [field: SerializeField] public int Stars { get; set; }
        [field: SerializeField] public int RequiredCharacterLevel { get; set; }
        [field: SerializeField] public int MinLevel { get; set; }
        [field: SerializeField] public int MaxLevel { get; set; }
        [field: SerializeField] public float ValuePerLvl { get; set; }
        [field: SerializeField] public AttributeWithValue[] Stats { get; set; }
        [field: SerializeField] public PassiveAbility[] Passives { get; set; } = Array.Empty<PassiveAbility>();
    }

    [CreateAssetMenu(menuName = "Gameplay/Equipment Database", fileName = "EquipmentDefDatabase", order = 0)]
    public class EquipmentDatabase : ScriptableObject
    {
        [Serializable]
        public struct Map
        {
            public string Id;
            public EquipmentData Data;
        }

        [SerializeField] private Map[] _maps = Array.Empty<Map>();
        public Map[] Maps => _maps;

        private readonly Dictionary<string, EquipmentData> _lookupDict = new();
        public Dictionary<string, EquipmentData> LookupDict => _lookupDict;

        private void OnEnable()
        {
            _lookupDict.Clear();
            foreach (var map in _maps) _lookupDict.Add(map.Id, map.Data);
        }
    }
}
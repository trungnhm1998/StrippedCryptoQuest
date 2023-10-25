using CryptoQuest.AbilitySystem.Abilities;
using IndiGames.Core.SaveSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    /// <summary>
    /// Define the equipment, stats, and other properties of an equipment
    /// </summary>
    [CreateAssetMenu(fileName = "Equipment", menuName = "Crypto Quest/Inventory/Equipment Def")]
    public class EquipmentDef : SerializableScriptableObject
    {
        [field: SerializeField] public string PrefabId { get; private set; }
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public bool IsNft { get; private set; }
        [field: SerializeField] public RaritySO Rarity { get; private set; }
        [field: SerializeField] public int Stars { get; private set; }
        [field: SerializeField] public int RequiredCharacterLevel { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public int SellPrice { get; private set; }
        [field: SerializeField] public int MinLevel { get; private set; }
        [field: SerializeField] public int MaxLevel { get; private set; }
        [field: SerializeField] public int RandomBonus { get; private set; }
        [field: SerializeField] public float ValuePerLvl { get; private set; }
        [field: SerializeField] public AttributeWithValue[] Stats { get; private set; }
        [field: SerializeField] public PassiveAbility Passive { get; private set; }
    }
}
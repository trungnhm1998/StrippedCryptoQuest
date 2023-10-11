using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Gameplay;
using UnityEngine;

namespace CryptoQuest.Character.Hero
{
    /// <summary>
    /// https://docs.google.com/spreadsheets/d/1WkX1DyDOGf6EiAppo8Buz2sUkSKV5OnDENEvmHzKXNQ/edit#gid=1523335650
    /// </summary>
    public class UnitSO : ScriptableObject, IUnit
    {
        [field: SerializeField] public string ID { get; private set; }
        [field: SerializeField] public IUnit.ECategory Category { get; private set; }
        [field: SerializeField] public Origin Origin { get; private set; }
        [field: SerializeField] public CharacterClass Class { get; private set; }
        [field: SerializeField] public Elemental Element { get; private set; }
        [field: SerializeField] public StatsDef Stats { get; private set; }
        [field: SerializeField] public bool IsNft { get; private set; }
    }
}
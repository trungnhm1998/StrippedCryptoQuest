using System;
using CryptoQuest.Character.Hero;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    /// <summary>
    /// Wrapper of hero spec in a party, only in party heroes able to equip items
    /// </summary>
    [Serializable]
    public class PartySlotSpec
    {
        [field: SerializeField] public HeroSpec Hero { get; set; } = new();

        [field: SerializeField] public Equipments EquippingItems { get; set; } = new();

        public bool IsValid() => Hero != null && Hero.IsValid();
    }

    public class PartySO : ScriptableObject
    {
        [SerializeField] private PartySlotSpec[] _heroSpecs;
        public PartySlotSpec[] GetParty() => _heroSpecs;

        public void SetParty(PartySlotSpec[] newSpecs)
        {
            _heroSpecs = newSpecs;
            Changed?.Invoke();
        }

        public event Action Changed;
    }
}
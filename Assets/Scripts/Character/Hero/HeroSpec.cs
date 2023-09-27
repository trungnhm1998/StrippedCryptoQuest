using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.Character.Hero
{
    /// <summary>
    /// Runtime hero data, use this to save game
    ///
    /// User should have bunch of Heroes in their profile
    ///
    /// Use the <see cref="AttributeSystemBehaviour"/> to get the runtime stats
    /// </summary>
    [Serializable]
    public struct HeroSpec
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public UnitSO Unit { get; set; }
        [field: SerializeField] public float Experience { get; set; }
        [field: SerializeField] public Equipments Equipments { get; set; }
        
        public bool IsValid() => Unit != null;
    }
}
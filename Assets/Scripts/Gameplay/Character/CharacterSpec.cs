using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Character
{
    /// <summary>
    /// Use this to save game
    /// </summary>
    [Serializable]
    public class CharacterSpec
    {
        [field: SerializeField] public HeroSO HeroDef { get; set; }
        [field: SerializeField] public CharacterClass Class { get; set; }
        [field: SerializeField] public Elemental Element { get; set; }
        [field: SerializeField] public int Level { get; set; }
        [field: SerializeField] public StatsDef StatsDef { get; set; }
    }
}
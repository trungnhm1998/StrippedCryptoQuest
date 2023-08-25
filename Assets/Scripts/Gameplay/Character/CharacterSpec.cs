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

        public float GetValueAtCurrentLevel(AttributeScriptableObject attributeDef)
        {
            // find the index of the attributeDef in the StatsDef
            var def = Array.Find(StatsDef.Attributes, v => v.Attribute == attributeDef);
            return GetValueAtLevel(Level, def);
        }

        public float GetValueAtCurrentLevel(int attributeDefIndex)
        {
            // find the index of the attributeDef in the StatsDef
            return GetValueAtLevel(Level, attributeDefIndex);
        }

        public float GetValueAtLevel(int currentLvl, int attributeDefIndex) =>
            GetValueAtLevel(currentLvl, StatsDef.Attributes[attributeDefIndex]);

        public float GetValueAtLevel(int currentLvl, CappedAttributeDef attributeDef)
        {
            var value = attributeDef.MinValue;

            value = Mathf.Floor((attributeDef.MaxValue - attributeDef.MinValue) / StatsDef.MaxLevel * currentLvl) +
                    attributeDef.MinValue;

            return value;
        }
    }
}
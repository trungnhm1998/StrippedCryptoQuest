using System;
using CryptoQuest.Character.Attributes;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Character
{
    public class CharacterSO : ScriptableObject
    {
        [Serializable]
        public struct Stats
        {
        }

        [field: SerializeField] public Stats DetailStats { get; set; }
        
         // TODO: Refactor below to use ID instead
        [field: SerializeField] public Elemental Element { get; set; }

        [field: SerializeField] public int CharClass { get; set; }
    }
}
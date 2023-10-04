using System;
using CryptoQuest.Character.Attributes;
using UnityEngine;

namespace CryptoQuest.Character
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
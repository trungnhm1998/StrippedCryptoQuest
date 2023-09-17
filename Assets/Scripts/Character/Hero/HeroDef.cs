using System;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.Character.Hero
{
    /// <summary>
    /// Defined data for hero their base stats, elements, job/class, personality, category
    /// </summary>
    [Serializable]
    public struct HeroData
    {
        public Origin Origin;
        public Elemental Element;
        public CharacterClass Class;
        public StatsDef Stats;
    }

    /// <summary>
    /// https://docs.google.com/spreadsheets/d/1WkX1DyDOGf6EiAppo8Buz2sUkSKV5OnDENEvmHzKXNQ/edit#gid=381366242
    /// https://docs.google.com/spreadsheets/d/1WkX1DyDOGf6EiAppo8Buz2sUkSKV5OnDENEvmHzKXNQ/edit#gid=1523335650
    /// </summary>
    public class HeroDef : CharacterData<HeroDef, HeroSpec>
    {
        [field: SerializeField] public HeroData Data { get; private set; }
    }
}
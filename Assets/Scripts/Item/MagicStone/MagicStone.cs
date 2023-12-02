using System;
using CryptoQuest.AbilitySystem.Abilities;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    public interface IMagicStone
    {
        public int ID { get; }
        public MagicStoneDef Definition { get; }
        public int Level { get; }
        public PassiveAbility[] Passives { get; }
        public bool IsValid();
    }
    
    [Serializable]
    public class MagicStone : IMagicStone
    {
        [field: SerializeField] public MagicStoneData StoneData { get; set; }
        public int ID => StoneData.ID;
        public MagicStoneDef Definition => StoneData.StoneDef;
        public int Level => StoneData.Level;
        public PassiveAbility[] Passives
        {
            get => StoneData.Passives;
            set => StoneData.Passives = value;
        }

        public bool IsValid()
        {
            return StoneData != null && StoneData.StoneDef != null;
        }
    }
}
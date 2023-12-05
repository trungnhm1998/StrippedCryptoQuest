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
        [field: SerializeField] public MagicStoneData Data { get; set; }
        public int ID => Data.ID;
        public MagicStoneDef Definition => Data.StoneDef;
        public int Level => Data.Level;
        public PassiveAbility[] Passives
        {
            get => Data.Passives;
            set => Data.Passives = value;
        }

        public bool IsValid()
        {
            return Data != null && Data.StoneDef != null;
        }
    }
}
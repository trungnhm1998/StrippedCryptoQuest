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
        public int AttachEquipmentId { get; }
        public PassiveAbility[] Passives { get; }
        public bool IsValid();
    }

    [Serializable]
    public class MagicStone : IMagicStone
    {
        [field: SerializeField] public MagicStoneData Data { get; set; }
        public int ID => Data.ID;
        public MagicStoneDef Definition => Data.Def;
        public int Level => Data.Level;
        public int AttachEquipmentId => Data.AttachEquipmentId;

        public PassiveAbility[] Passives
        {
            get => Data.Passives;
            set => Data.Passives = value;
        }

        public bool IsValid()
        {
            return Data != null && Data.Def != null;
        }
    }
}
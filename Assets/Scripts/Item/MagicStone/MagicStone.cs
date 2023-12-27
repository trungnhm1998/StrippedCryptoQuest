using System;
using CryptoQuest.AbilitySystem.Abilities;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    public interface IMagicStone
    {
        int ID { get; }
        MagicStoneDef Definition { get; }
        int Level { get; }
        int AttachEquipmentId { get; set; }
        PassiveAbility[] Passives { get; }
        bool IsValid();
    }

    [Serializable]
    public class MagicStone : IMagicStone
    {
        [field: SerializeField] public MagicStoneData Data { get; set; }
        public int ID => Data.ID;
        public MagicStoneDef Definition => Data.Def;
        public int Level => Data.Level;
        public int AttachEquipmentId 
        {
            get => Data.AttachEquipmentId;
            set => Data.AttachEquipmentId = value;
        }

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
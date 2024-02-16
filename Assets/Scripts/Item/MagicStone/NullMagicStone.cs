using System;
using CryptoQuest.AbilitySystem.Abilities;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    [Serializable]
    public class NullMagicStone : IMagicStone
    {
        private static NullMagicStone _instance;
        public static NullMagicStone Instance => _instance ??= new NullMagicStone();
        public int ID => -1;
        public MagicStoneDef Definition { get; set; }
        public int Level => -1;
        public int AttachEquipmentId { get; set; }
        public PassiveAbility[] Passives => Array.Empty<PassiveAbility>();

        public bool IsValid() => Definition != null;
        public bool IsEqual(IMagicStone other) => false;
    }
}
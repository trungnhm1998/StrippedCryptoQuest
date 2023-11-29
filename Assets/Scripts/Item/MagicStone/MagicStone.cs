using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Gameplay.Inventory;

namespace CryptoQuest.Item.MagicStone
{
    [Serializable]
    public class MagicStone
    {
        public MagicStoneData StoneData { get; set; }
        public int Level => StoneData.Level;
        public PassiveAbility[] Passives => StoneData.Passives;

        public bool IsValid()
        {
            return StoneData != null && StoneData.stoneDef != null;
        }
    }
}
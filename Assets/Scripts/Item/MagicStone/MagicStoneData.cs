using System;
using CryptoQuest.AbilitySystem.Abilities;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    [Serializable]
    public class MagicStoneData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public MagicStoneDef stoneDef { get; set; }
        [field: SerializeField] public int Level { get; set; }
        [field: SerializeField] public PassiveAbility[] Passives { get; set; } = Array.Empty<PassiveAbility>();
    }
}
using System;
using CryptoQuest.AbilitySystem.Abilities;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    [Serializable]
    public class MagicStoneData
    {
        [field: SerializeField] public int ID { get; set; }
        [field: SerializeField] public MagicStoneDef Def { get; set; }
        [field: SerializeField] public int Level { get; set; }
        [field: SerializeField] public int AttachEquipmentId { get; set; }
        [field: SerializeField] public PassiveAbility[] Passives { get; set; } = Array.Empty<PassiveAbility>();
    }
}
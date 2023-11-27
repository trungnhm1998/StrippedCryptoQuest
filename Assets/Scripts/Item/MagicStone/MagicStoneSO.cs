using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    public class MagicStoneSO : GenericItem
    {
        [field: SerializeField] public Elemental Element { get; private set; }
        [field: SerializeField] public List<PassiveAbility> PassiveAbilities { get; private set; } = new();
    }
}
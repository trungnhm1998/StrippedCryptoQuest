using System;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Gameplay.Inventory;
using UnityEngine;

namespace CryptoQuest.Item.MagicStone
{
    public class MagicStone : GenericItem
    {
        [field: SerializeField] public Elemental Element { get; private set; }
        [field: SerializeField] public List<PassiveAbility> PassiveAbilities { get; private set; } = new();
    }

    [Serializable]
    public class MagicStoneInfo : ItemInfo<MagicStone>
    {
        public override bool AddToInventory(IInventoryController inventory)
            => inventory.Add(this);

        public override bool RemoveFromInventory(IInventoryController inventory)
            => inventory.Remove(this);
    }
}
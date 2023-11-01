
using System;
using CryptoQuest.Gameplay.Loot;
using UnityEngine;

namespace CryptoQuest.Character.Enemy
{
    public interface IDropToAdd
    {
        LootInfo Loot { get; }
    }

    [Serializable]
    public class ConsumeDrop : IDropToAdd
    {
        [SerializeField] private UsableLootInfo _loot;
        public LootInfo Loot => _loot;
    }

    [Serializable]
    public class EquipmentDrop : IDropToAdd
    {
        [SerializeField] private EquipmentLootInfo _loot;
        public LootInfo Loot => _loot;
    }

    [Serializable]
    public class CurrencyDrop : IDropToAdd
    {
        [SerializeField] private CurrencyLootInfo _loot;
        public LootInfo Loot => _loot;
    }

    [Serializable]
    public class ExpDrop : IDropToAdd
    {
        [SerializeField] private ExpLoot _loot;
        public LootInfo Loot => _loot;
    }
}
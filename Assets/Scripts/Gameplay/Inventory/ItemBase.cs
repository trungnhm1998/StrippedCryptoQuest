using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public abstract class ItemBase
    {
        public int Quantity;
        public ItemSO ItemSO;

        public ItemBase(ItemSO itemSO, int quantity = 0)
        {
            ItemSO = itemSO;
            Quantity = quantity;
        }

        public abstract void Use();
    }
}
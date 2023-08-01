using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public class ItemInfomation
    {
        public int Quantity;
        public ItemSO ItemSO;
        public AbilitySystemBehaviour Owner;

        public ItemInfomation(ItemSO itemSO, int quantity = 0)
        {
            ItemSO = itemSO;
            Quantity = quantity;
        }

        public ItemInfomation(AbilitySystemBehaviour owner, ItemSO itemSO, int quantity = 0)
        {
            ItemSO = itemSO;
            Quantity = quantity;
            Owner = owner;
        }

        public virtual void Use() { }
    }
}
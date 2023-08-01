using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public class ExpendableItemInfo : ItemInfomation
    {
        private new ExpendableItemSO ItemSO => (ExpendableItemSO)base.ItemSO;
        public ExpendableItemInfo(ItemSO itemSO, int quantity = 0) : base(itemSO, quantity) { }

        public ExpendableItemInfo(AbilitySystemBehaviour owner, ItemSO itemSO, int quantity = 0) : base(owner, itemSO,
            quantity) { }

        public override void Use()
        {
            if (Owner == null) return;
            var ability = Owner.GiveAbility(ItemSO.Ability);
            Owner.TryActiveAbility(ability);
        }
    }
}
using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [Serializable]
    public class UsableInfo : ItemInfo
    {
        [field: SerializeField] public int Quantity { get; private set; }
        public AbilitySystemBehaviour Owner { get; set; }

        [field: SerializeField] public UsableSO Item { get; private set; }

        public UsableInfo(UsableSO baseItemSO, int quantity = 0) : base(baseItemSO)
        {
            Quantity = quantity;
        }

        public UsableInfo(UsableSO baseItemSO, AbilitySystemBehaviour owner, int quantity = 0) : base(baseItemSO)
        {
            Quantity = quantity;
            Owner = owner;
        }

        public UsableInfo(ItemGenericSO baseItemSO, int quantity)
        {
            Item = baseItemSO as UsableSO;
            Quantity = quantity;
        }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
        }

        protected override void Activate()
        {
            if (Owner == null) return;
            AbstractAbility ability = Owner.GiveAbility(Item.Ability);
            Owner.TryActiveAbility(ability);
        }

        public void UseItem() => Activate();
    }
}
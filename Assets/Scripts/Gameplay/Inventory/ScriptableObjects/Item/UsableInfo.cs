using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [Serializable]
    public class UsableInfo : ItemInfo
    {
        [field: SerializeField] public int Quantity { get; private set; } = 1;
        public AbilitySystemBehaviour Owner { get; set; }

        [field: SerializeField] public UsableSO Item { get; private set; }

        public Sprite Icon => Item.Image;
        public LocalizedString DisplayName => Item.DisplayName;
        public LocalizedString Description => Item.Description;

        public UsableInfo(UsableSO baseItemSO, int quantity = 0) : base(baseItemSO)
        {
            Item = baseItemSO;
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

        public UsableInfo() { }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
        }

        protected override void Activate()
        {
            if (Owner == null) return;
            GameplayAbilitySpec gameplayAbilitySpec = Owner.GiveAbility(Item.Ability);
            Owner.TryActiveAbility(gameplayAbilitySpec);
        }

        public void UseItem() => Activate();

        public void Use()
        {
            var action = Item.Action;
            action.ActionContext = new ActionSpecificationBase.Context()
            {
                Item = this
            };
            action.Execute();
        }
    }
}
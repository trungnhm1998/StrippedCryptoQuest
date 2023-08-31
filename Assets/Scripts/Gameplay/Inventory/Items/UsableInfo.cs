using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.ActionTypes;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Inventory.Items
{
    [Serializable]
    public class UsableInfo : ItemInfo<UsableSO>
    {
        [field: SerializeField] public int Quantity { get; private set; } = 1;
        public AbilitySystemBehaviour Owner { get; set; }

        public Sprite Icon => Data.Image;
        public LocalizedString DisplayName => Data.DisplayName;
        public LocalizedString Description => Data.Description;

        public UsableInfo(UsableSO baseItemSO, int quantity = 0) : base(baseItemSO)
        {
            Quantity = quantity;
        }

        public UsableInfo(UsableSO baseItemSO, AbilitySystemBehaviour owner, int quantity = 0) : base(baseItemSO)
        {
            Quantity = quantity;
            Owner = owner;
        }

        public UsableInfo() { }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
        }

        private void Activate()
        {
            if (Owner == null) return;
            GameplayAbilitySpec gameplayAbilitySpec = Owner.GiveAbility(Data.Ability);
            Owner.TryActiveAbility(gameplayAbilitySpec);
        }

        public void UseItem() => Activate();

        public void Use()
        {
            var action = Data.Action;
            action.ActionContext = new ActionSpecificationBase.Context()
            {
                Item = this
            };
            action.Execute();
        }
    }
}
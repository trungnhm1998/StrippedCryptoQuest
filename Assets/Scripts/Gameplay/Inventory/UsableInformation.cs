using System;
using CryptoQuest.Data.Item;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public class UsableInformation : ItemInformation
    {
        [field: SerializeField] public int Quantity { get; private set; }
        [HideInInspector] public AbilitySystemBehaviour Owner;

        [field: SerializeField] public UsableSO ItemSO { get; private set; }

        public UsableInformation(UsableSO itemSo, int quantity = 0) : base(itemSo)
        {
            Quantity = quantity;
        }

        public UsableInformation(UsableSO itemSo, AbilitySystemBehaviour owner, int quantity = 0) : base(itemSo)
        {
            Quantity = quantity;
            Owner = owner;
        }

        protected override void Activate()
        {
            Debug.Log($"Activate {ItemSO.name}");
            if (Owner == null) return;
            Debug.Log($"Owner {Owner.name}");
            AbstractAbility ability = Owner.GiveAbility(ItemSO.Ability);
            Owner.TryActiveAbility(ability);
        }

        public void UseItem() => Activate();
    }
}
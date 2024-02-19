using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.Consumable;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Inventory
{
    public class ConsumeItemOnPartyController : SagaBase<ConsumeItemOnParty>
    {
        [SerializeField] private PartyManager _party;

        protected override void HandleAction(ConsumeItemOnParty ctx)
        {
            bool ableToUseOnAtLeastOneHero = false;
            ConsumableSO item = ctx.Item;
            foreach (var slot in _party.Slots)
            {
                if (slot.IsValid() == false) continue;
                var hero = slot.HeroBehaviour;
                var spec = hero.AbilitySystem.GiveAbility<ConsumableAbilitySpec>(item.Ability);

                if (spec.CanActiveAbility())
                {
                    ableToUseOnAtLeastOneHero = true;
                    Debug.Log($"Consuming {item} on {hero.GetInstanceID()}");
                    spec.TryActiveAbility(item);
                }
            }

            if (ableToUseOnAtLeastOneHero == false) return;
            ActionDispatcher.Dispatch(new ItemConsumed(item));
        }
    }
}
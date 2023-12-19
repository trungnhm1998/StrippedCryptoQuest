using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Inventory.Actions;
using IndiGames.Core.Events;

namespace CryptoQuest.Inventory
{
    public class ConsumeItemOnCharacterController : SagaBase<ConsumeItemOnCharacter>
    {
        protected override void HandleAction(ConsumeItemOnCharacter ctx)
        {
            var hero = ctx.Hero;
            var item = ctx.Item;
            if (hero.IsValid() == false) return;

            var abilitySpec = hero.AbilitySystem.GiveAbility<ConsumableAbilitySpec>(item.Ability);
            if (abilitySpec.CanActiveAbility() == false) return;
            abilitySpec.TryActiveAbility(item);
            ActionDispatcher.Dispatch(new ItemConsumed(item));
        }
    }
}
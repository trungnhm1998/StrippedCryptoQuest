using System;
using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Character.Ability
{
    public class SingleHeroConsumeAbility : ConsumeItemAbility
    {
        public static event Action ShowHeroSelection;
        protected override GameplayAbilitySpec CreateAbility() => new SingleTargetConsumeAbilitySpec(this);
        public override void Consuming() => ShowHeroSelection?.Invoke();

        private class SingleTargetConsumeAbilitySpec : ConsumableAbilitySpec
        {
            private readonly SingleHeroConsumeAbility _def;

            public SingleTargetConsumeAbilitySpec(SingleHeroConsumeAbility def)
            {
                _def = def;
            }

            protected override IEnumerator OnAbilityActive()
            {
                throw new NotImplementedException();
            }

            public override void Consume(params AbilitySystemBehaviour[] targets)
            {
            }
        }
    }
}
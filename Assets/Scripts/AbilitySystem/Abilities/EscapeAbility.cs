using CryptoQuest.Core;
using CryptoQuest.Gameplay.Manager;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.AbilitySystem.Abilities
{
    public class EscapeAbility : CastSkillAbility
    {
        protected override GameplayAbilitySpec CreateAbility() =>
            new EscapeAbilitySpec(this);
    }

    public class EscapeAbilitySpec : CastSkillAbilitySpec
    {
        public EscapeAbilitySpec(EscapeAbility def) : base(def) { }

        protected override void InternalExecute(AbilitySystemBehaviour target)
        {
            ActionDispatcher.Dispatch(new EscapeAction());
        }
    }
}
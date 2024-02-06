using System;
using IndiGames.Core.Events;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.AbilitySystem.Abilities.Conditions
{
    /// <summary>
    /// Raise event when condition is failed.
    /// </summary>
    [Serializable]
    public class PassAtleastOneTagWithEvent : PassAtleastOneTag 
    {
        public override bool IsPass(AbilityConditionContext ctx)
        {
            var isPass = base.IsPass(ctx);

            if (!isPass)
                ActionDispatcher.Dispatch(new AbilityConditionFailed(ctx.System));
        
            return isPass;
        }
    }

    public class AbilityConditionFailed : ActionBase 
    {
        public AbilitySystemBehaviour Owner { get; private set; }
        public AbilityConditionFailed(AbilitySystemBehaviour owner) 
            => Owner = owner;
    }
}
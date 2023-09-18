using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.Implementation.BasicEffect
{
    [CreateAssetMenu(fileName = "InfiniteEffect", menuName = "Indigames Ability System/Effects/Infinite Effect")]
    public class InfiniteEffectScriptableObject : EffectScriptableObject<InfiniteEffectSpec> { }

    public class InfiniteEffectSpec : GameplayEffectSpec
    {
        public override ActiveEffectSpecification Accept(IEffectApplier effectApplier)
        {
            return effectApplier.Visit(this);
        }

        public override void Update(float deltaTime) { }
    }
}
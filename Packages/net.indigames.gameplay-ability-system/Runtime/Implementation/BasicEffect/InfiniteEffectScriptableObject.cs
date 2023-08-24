using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.Implementation.BasicEffect
{
    [CreateAssetMenu(fileName = "InfiniteEffect", menuName = "Indigames Ability System/Effects/Infinite Effect")]
    public class InfiniteEffectScriptableObject : EffectScriptableObject<InfiniteEffectSpec> { }

    public interface IInfiniteEffectSpec : IGameplayEffectSpec { }

    public class InfiniteEffectSpec : GameplayEffectSpec, IInfiniteEffectSpec
    {
        public override ActiveEffectSpecification Accept(IEffectApplier effectApplier)
        {
            return effectApplier.Visit(this);
        }

        public override void Update(float deltaTime) { }
    }
}
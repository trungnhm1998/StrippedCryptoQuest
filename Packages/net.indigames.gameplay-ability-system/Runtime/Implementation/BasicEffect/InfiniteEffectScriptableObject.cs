using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.Implementation.BasicEffect
{
    [CreateAssetMenu(fileName = "InfiniteEffect", menuName = "Indigames Ability System/Effects/Infinite Effect")]
    public class InfiniteEffectScriptableObject : EffectScriptableObject<InfiniteEffect> {}

    public class InfiniteEffect : AbstractEffect
    {
        public override void Accept(IEffectApplier effectApplier)
        {
            effectApplier.ApplyDurationalEffect(this);
        }

        public override void Update(float deltaTime)
        {
        }
    }
}
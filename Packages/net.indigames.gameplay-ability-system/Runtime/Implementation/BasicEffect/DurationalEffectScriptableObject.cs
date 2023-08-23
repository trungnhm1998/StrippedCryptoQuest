using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.Implementation.BasicEffect
{
    [CreateAssetMenu(fileName = "DurationalEffect", menuName = "Indigames Ability System/Effects/Durational Effect")]
    public class DurationalEffectScriptableObject : EffectScriptableObject<DurationalEffectSpec>
    {
        public float Duration;
    }

    /// <summary>
    /// Effect need update over time such as slow or stun enemy for 3 seconds
    /// </summary>
    public class DurationalEffectSpec : GameplayEffectSpec
    {
        private float _remainingDuration;

        public float RemainingDuration
        {
            get => _remainingDuration;
            set => _remainingDuration = value;
        }

        public override void InitEffect(EffectScriptableObject effectDef, AbilitySystemBehaviour source)
        {
            base.InitEffect(effectDef, source);
            _remainingDuration = ((DurationalEffectScriptableObject) effectDef).Duration;
        }

        public override void Accept(IEffectApplier effectApplier)
        {
            base.Accept(effectApplier);
            _effectApplier.Visit(this);
        }

        public override void Update(float deltaTime)
        {
            _remainingDuration -= deltaTime;
            if (_remainingDuration <= 0)
            {
                IsExpired = true;
            }
        }
    }
}
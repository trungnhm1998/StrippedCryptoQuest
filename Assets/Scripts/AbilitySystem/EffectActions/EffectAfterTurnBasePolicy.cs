using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.EffectActions
{
    [Serializable]
    public class EffectAfterTurnBasePolicy : TurnBasePolicy
    {
        [SerializeField] private GameplayEffectDefinition[] _afterEffects;
        public GameplayEffectDefinition[] AfterEffects => _afterEffects;

        public override ActiveGameplayEffect CreateActiveEffect(GameplayEffectSpec inSpec) =>
            new EffectAfterTurnBasePolicyActiveEffect(this, inSpec);
    }

    /// <summary>
    /// This action depends on <see cref="CastSkillAbility"/> in order to work.
    /// </summary>
    [Serializable]
    public class EffectAfterTurnBasePolicyActiveEffect : TurnBasePolicyActiveEffect
    {
        private EffectAfterTurnBasePolicy _policyDef;

        public EffectAfterTurnBasePolicyActiveEffect(EffectAfterTurnBasePolicy policyDef,
            GameplayEffectSpec spec) : base(policyDef, spec)
        {
            _policyDef = policyDef;
        }

        public override void OnRemoved()
        { 
            base.OnRemoved();
            ApplyAfterEffect();
        }

        private void ApplyAfterEffect()
        {
            foreach (var effectDef in _policyDef.AfterEffects)
            {
                var effect = _character.AbilitySystem.MakeOutgoingSpec(effectDef);
                _character.ApplyEffect(effect);
            }
        }
    }
}